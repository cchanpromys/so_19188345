using System;
using System.Configuration;
using System.IO;
using System.Threading;
using ProtoBuf;

namespace CollectionTest.Helper
{
    public interface IEntity
    {
        string Id { get; set; }
    }

    public interface IProtobufStore
    {
        T Get<T>(string id);
        void Store(IEntity doc);
    }

    public class ProtobufStore : IProtobufStore
    {
        private readonly string protobufPath;

        public ProtobufStore()
        {
            protobufPath = ConfigurationManager.AppSettings["ProtobufPath"];

            if (protobufPath == null)
                throw new Exception("Defind ProtobufPath in Web.config or App.config");

            if (!protobufPath.EndsWith("/"))
                protobufPath = protobufPath + "/";
        }

        public T Get<T>(string id)
        {
            var path = GetPath(id);
            Directory.CreateDirectory(path);

            path = path + "/" + GetFile(id);

            if (!File.Exists(path))
                return default(T);

            T result = default(T);

            FileAccessWait(() =>
                {
                    using (var file = File.OpenRead(path))
                    {
                        result = Serializer.Deserialize<T>(file);
                    }
                });

            return result;
        }

        public void Store(IEntity doc)
        {
            if (doc == null)
                return;

            var path = GetPath(doc.Id);
            Directory.CreateDirectory(path);

            path = path + "/" + GetFile(doc.Id);

            if (File.Exists(path))
                FileAccessWait(() => File.Delete(path));

            FileAccessWait(() =>
                {
                    using (var file = File.Create(path))
                    {
                        Serializer.Serialize(file, doc);
                    }
                });

        }

        private string GetPath(string id)
        {
            var index = id.IndexOf("-", StringComparison.Ordinal);

            if(index < 0)
                index = id.IndexOf("/", StringComparison.Ordinal);

            if (index > 0)
                id = id.Substring(0, index);

            return protobufPath + id;
        }

        private string GetFile(string id)
        {
            return id.Substring(id.LastIndexOf("/", StringComparison.Ordinal) + 1);
        }

        private void FileAccessWait(Action fileAction)
        {
            const int maxRetry = 100;
            int retry = 0;

            while (true)
            {
                try
                {
                    fileAction.Invoke();
                    return;
                }
                catch (IOException)
                {
                    if (retry > maxRetry)
                        throw;

                    Thread.Sleep(500);
                }
                catch (UnauthorizedAccessException){
                    if (retry > maxRetry)
                        throw;

                    Thread.Sleep(500);
                }
                retry++;
            }
        }

        protected virtual bool IsFileLocked(string path)
        {
            var file = new FileInfo(path);

            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}
