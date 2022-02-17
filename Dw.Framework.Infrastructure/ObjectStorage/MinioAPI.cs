using Dw.Framework.Infrastructure.Utility;
using Minio;
using Minio.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.Infrastructure.ObjectStorage
{
    /// <summary>
    /// minio的api，用于实现对minio的操作
    /// </summary>
    public class MinioAPI
    {

        // 定义一个静态变量来保存类的实例
        // volatile在代码被编译的时候不会微调代码
        private static volatile MinioAPI uniqueInstance;

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();


        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// 设置Global.host  Global.accessKey  Global.secretKey
        /// </summary>
        /// <returns></returns>
        public static MinioAPI GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            // 双重锁定只需要一句判断就可以了
            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new MinioAPI();
                    }
                }
            }
            return uniqueInstance;
        }


        /**
         * 集群节点中任何一个节点的ip即可，因为minio是无中心的
         */
        private string host = "";

        /**
         * 等同于用户名，每个用户需要创建accessKey与secretKey才可以登陆系统
         */
        private string accessKey = "";

        /**
         * 等同于密码，每个用户需要创建accessKey与secretKey才可以登陆系统
         */
        private string secretKey = "";

        //连接客户端
        MinioClient minioClient = null;

        /**
         * 构造方法
         * // 定义私有构造函数，使外界不能创建该类实例
         */
        public MinioAPI()
        {
            host = Appsettings.App("MinioConfig", "Endpoint");
            secretKey = Appsettings.App("MinioConfig", "SecretKey");
            accessKey = Appsettings.App("MinioConfig", "AccessKey");
            Init();
        }

        /**
         * 重新初始化连接，每次调用时都会新建一个实例
         */
        public void Init()
        {
            minioClient = new MinioClient(host, accessKey, secretKey);
        }

        /**
         * 创建桶
         *
         * @param bucketName 桶名称
         */

        public async Task CreateBucketAsync(string bucketName)
        {
            if (!await minioClient.BucketExistsAsync(bucketName))
            {
                await minioClient.MakeBucketAsync(bucketName);
            }
        }

        /**
         * 获取全部桶
         *
         * @return 返回minio集群中一共有哪些桶
         * https://docs.minio.io/cn/java-client-api-reference.html#listBuckets
         */
        public async Task<List<Bucket>> GetAllBucketsAsync()
        {
            return (await minioClient.ListBucketsAsync()).Buckets;
        }

        /**
         * 上传文件,通过流的方式，文件类型为流
         * 如果大于5MB，将自动分片后通过多会话上传
         * 可以直接获取request请求中的流或者数据库中的流进行操作,性能更高
         *
         * @param bucketName 桶名称
         * @param objectName 文件名称
         * @param stream     文件流
         * @throws Exception https://docs.minio.io/cn/java-client-api-reference.html#putObject
         */
        public async Task UploadByStreamAsync(string bucketName, string objectName, Stream stream)
        {
            await minioClient.PutObjectAsync(bucketName, objectName, stream, stream.Length, "application/octet-stream");

        }

        /**
         * 上传文件,通过流的方式，指定流大小与文件类型
         * 如果大于5MB，将自动分片后通过多会话上传
         * 可以直接获取request请求中的流或者数据库中的流进行操作,性能更高
         *
         * @param bucketName  桶名称
         * @param objectName  文件名称
         * @param stream      文件流
         * @param size        大小
         * @param contextType 类型
         * @throws Exception https://docs.minio.io/cn/java-client-api-reference.html#putObject
         */
        public async Task UploadByStreamAsync(string bucketName, string objectName, Stream stream, long size, string contextType)
        {
            await minioClient.PutObjectAsync(bucketName, objectName, stream, size, contextType);
        }


        /**
         * 上传文件,通过文件的方式，指定本地文件的路径
         * 如果对象大于5MB，将自动分片后通过多会话上传
         * 会直接读取文件的信息
         *
         * @param bucketName 桶名称
         * @param objectName 文件名称
         * @param filePath   文件路径
         * @throws Exception https://docs.minio.io/cn/java-client-api-reference.html#putObject
         */
        public async Task UploadByFileAsync(string bucketName, string objectName, string filePath)
        {
            await minioClient.PutObjectAsync(bucketName, objectName, filePath);
        }


        /**
         * 获取文件信息
         *
         * @param bucketName 桶名称
         * @param objectName 文件名称
         * @return 包含了该文件的基本信息
         * @throws Exception https://docs.minio.io/cn/java-client-api-reference.html#statObject
         */
        public async Task<ObjectStat> GetObjectInfoAsync(string bucketName, string objectName)
        {
            return await minioClient.StatObjectAsync(bucketName, objectName);
        }

        /**
         * 删除文件
         *
         * @param bucketName 桶名称
         * @param objectName 文件名称
         * @throws Exception https://docs.minio.io/cn/java-client-api-reference.html#removeObject
         */
        public async Task removeObjectAsync(string bucketName, string objectName)
        {
            await minioClient.RemoveObjectAsync(bucketName, objectName);
        }


        /**
         * 从集群中下载文件到本地
         * 如果要实现下载功能，本方法性能略低，因为要多写入一次服务器文件系统，推荐使用getStream()以流的方式直接输出
         *
         * @param bucketName 桶名称
         * @param objectName 文件名称
         * @param filePath   文件路径
         * @throws Exception https://docs.minio.io/cn/java-client-api-reference.html#putObject
         */
        public async Task DownloadToFileAsync(string bucketName, string objectName, string filePath)
        {
            await minioClient.GetObjectAsync(bucketName, objectName, filePath);
        }

        /**
         * 从集群中下载文件到本地
         * 如果要实现下载功能，本方法性能略低，因为要多写入一次服务器文件系统，推荐使用getStream()以流的方式直接输出
         *
         * @param bucketName 桶名称
         * @param objectName 文件名称
         * @throws Exception https://docs.minio.io/cn/java-client-api-reference.html#putObject
         */
        public async Task getObjectAsync(string bucketName, string objectName)
        {
            await minioClient.GetObjectAsync(bucketName, objectName, objectName);
        }

        /**
         * 获取文件的流
         * 推荐实现下载功能时通过此方法直接获取流进行输出，提升性能
         *
         * @param bucketName 桶名称
         * @param objectName 文件名称
         * @return 对象的文件流，可以用于文件保存，或者输出到服务端
         * @throws Exception https://docs.minio.io/cn/java-client-api-reference.html#putObject
         */
        public async Task<Stream> GetStreamAsync(string bucketName, string objectName)
        {
            Stream fileStream = null;
            await minioClient.GetObjectAsync(bucketName, objectName, (stream) =>
            {
                stream.CopyTo(Console.OpenStandardOutput());
                fileStream = stream;
            });
            return fileStream;
        }

        /**
         * 获取文件的流,支持断点下载，如指定下载1024-2048范围内的数据
         * 推荐实现下载功能时通过此方法直接获取流进行输出，提升性能
         * 通过偏移量和长度来实现断点下载
         *
         * @param bucketName 桶名称
         * @param objectName 文件名称
         * @param offset     偏移量
         * @param length     下载总长度
         * @return 对象的文件流，可以用于文件保存，或者输出到服务端
         * @throws Exception https://docs.minio.io/cn/java-client-api-reference.html#putObject
         */
        public async Task<Stream> getStreamAsync(string bucketName, string objectName, long offset, long length)
        {
            Stream fileStream = null;
            await minioClient.GetObjectAsync(bucketName, objectName, offset, length, (stream) =>
            {
                stream.CopyTo(Console.OpenStandardOutput());
                fileStream = stream;
            });
            return fileStream;
        }

        /**
         * 查找文件
         * for (Result<Item> result : myObjects) {
         * Item item = result.get();
         * System.out.println(item.lastModified() + ", " + item.size() + ", " + item.objectName());
         * }
         *
         * @param buckName 桶名称
         * @param prefix   文件名前缀
         * @return 文件信息
         * @throws Exception
         */
        public IObservable<Item> Find(string buckName, string prefix)
        {
            IObservable<Item> myObjects = minioClient.ListObjectsAsync(buckName, prefix);

            return myObjects;
        }

        /**
         * 复制,并重命名
         *
         * @param sourceBuckName   源桶
         * @param sourceObjectName 源文件
         * @param destBuckName     目标桶
         * @param destObjectName   目标文件
         * @throws Exception
         */
        public async Task CopyAsync(string sourceBuckName, string sourceObjectName, string destBuckName, string destObjectName)
        {
            await minioClient.CopyObjectAsync(sourceBuckName, sourceObjectName, destBuckName, destObjectName);
        }

        /**
         * 复制
         *
         * @param sourceBuckName   源桶
         * @param sourceObjectName 源文件
         * @param destBuckName     目标桶
         * @throws Exception
         */
        public async Task CopyAsync(string sourceBuckName, string sourceObjectName, string destBuckName)
        {
            await minioClient.CopyObjectAsync(sourceBuckName, sourceObjectName, destBuckName);
        }

        /**
         * 获取文件外链
         *
         * @param bucketName bucket名称
         * @param objectName 文件名称
         * @return url
         */
        public async Task<string> GetObjectURLAsync(string bucketName, string objectName)
        {
            return await minioClient.PresignedGetObjectAsync(bucketName, objectName, 60 * 60 * 60 * 7);
        }


        /**
         * 剪切
         * 先复制，后删除
         *
         * @param sourceBuckName   源桶
         * @param sourceObjectName 源文件
         * @param destBuckName     目标桶
         * @param destObjectName   目标文件
         * @throws Exception
         */
        public async Task cutAsync(string sourceBuckName, string sourceObjectName, string destBuckName, string destObjectName)
        {
            await CopyAsync(sourceBuckName, sourceObjectName, destBuckName, destObjectName);
            await minioClient.RemoveObjectAsync(sourceBuckName, sourceObjectName);
        }

        /**
         * 删除
         *
         * @param buckName   桶
         * @param objectName 文件
         * @throws Exception
         */
        public async Task deleteAsync(string buckName, string objectName)
        {

            await minioClient.RemoveObjectAsync(buckName, objectName);
        }

        #region
        ///**
        // * 上传文件,通过流的方式，文件类型为流
        // * 手动控制分片，支持断点续传,全部上传完成后需要通过,commitMultUpload提交，如果放弃需要通过rollbackMultUpload提交
        // * 可以直接获取request请求中的流或者数据库中的流进行操作,性能更高
        // *
        // * @param bucketName 桶名称
        // * @param objectName 文件名称
        // * @param stream     文件流
        // */
        //public async Task<Dictionary<string, object>> MultUploadByStreamAsync(string uploadId, string bucketName, string objectName, Stream stream, long size, Dictionary<string, string> headerDictionary, string contentType, Part[] parts)
        //{
        //    //执行分片上传的主id，后续用来销毁或者提交都需要传入
        //    if (uploadId == null)
        //    {
        //        uploadId = await minioClient.InitMultUploadAsync(bucketName, objectName, headerDictionary, contentType);
        //    }
        //    parts = await minioClient.MakeMultUploadAsync(uploadId, bucketName, objectName, size, stream, parts);
        //    Dictionary<string, object> returnDictionary = new Dictionary<string, object>();
        //    returnDictionary.Add("id", uploadId);
        //    returnDictionary.Add("parts", parts);
        //    //检查parts状态
        //    if (parts != null && parts.Length > 0)
        //    {
        //        returnDictionary.Add("result", 0);
        //        foreach (Part p in parts)
        //        {
        //            if (p == null)
        //            {// || p.getState() != 0) {
        //                returnDictionary.Add("result", -1);
        //                break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        returnDictionary.Add("result", -1);
        //    }
        //    return returnDictionary;
        //}


        ///**
        // * 上传文件,通过流的方式，文件类型为流
        // * 手动控制分片，支持断点续传,全部上传完成后需要通过,commitMultUpload提交，如果放弃需要通过rollbackMultUpload提交
        // * 可以直接获取request请求中的流或者数据库中的流进行操作,性能更高
        // *
        // * @param bucketName 桶名称
        // * @param objectName 文件名称
        // */
        //public async Task<Dictionary<string, object>> MultUploadByStreamAsync(string bucketName, string objectName, long size,
        //                                              Dictionary<string, string> headerDictionary, string contentType, Part[] parts)
        //{
        //    Dictionary<string, Object> Dictionary = new Dictionary<string, object>();
        //    //执行分片上传的主id，后续用来销毁或者提交都需要传入
        //    string uploadId = await minioClient.InitMultUploadAsync(bucketName, objectName, headerDictionary, contentType);
        //    parts = minioClient.MakeMultUpload(size, parts);
        //    Dictionary.Add("uploadId", uploadId);
        //    Dictionary.Add("parts", parts);
        //    return Dictionary;
        //}

        ///**
        // * 上传文件,通过流的方式，文件类型为流
        // * 手动控制分片，支持断点续传,全部上传完成后需要通过,commitMultUpload提交，如果放弃需要通过rollbackMultUpload提交
        // * 可以直接获取request请求中的流或者数据库中的流进行操作,性能更高
        // *
        // * @param bucketName 桶名称
        // * @param objectName 文件名称
        // * @param stream     文件流
        // */
        //public async Task<Part[]> MultUploadByStreamAsync(string uploadId, string bucketName, string objectName, Stream stream, long expectedReadSize, Part[] parts, int partNumber)
        //{
        //    //Dictionary<string, object> returnDictionary = new Dictionary<string, object>();
        //    return await minioClient.MakeMultUploadAsync(uploadId, bucketName, objectName, expectedReadSize, stream, parts, partNumber);
        //}


        ///**
        // * 提交分段上传文件
        // *
        // * @param bucketName 桶名称
        // * @param objectName 文件名称
        // */
        //public async Task CommitMultUploadAsync(string uploadId, string bucketName, string objectName, Part[] parts)
        //{
        //    await minioClient.CommitMultUploadAsync(uploadId, bucketName, objectName, parts);
        //}

        /**
         * 放弃分段上传文件
         *
         * @param bucketName 桶名称
         * @param objectName 文件名称
         */
        public void rollbackMultUpload(string uploadId, string bucketName, string objectName)
        {
            // minioClient.abortMultipartUpload(bucketName, objectName, uploadId);
        }
        #endregion

        public string GetAccessKey()
        {
            return accessKey;
        }

        public void SetAccessKey(string accessKey)
        {
            this.accessKey = accessKey;
        }

        public string getHost()
        {
            return host;
        }

        public void SetHost(string host)
        {
            this.host = host;
        }

        public string GetSecretKey()
        {
            return secretKey;
        }

        public void SetSecretKey(string secretKey)
        {
            this.secretKey = secretKey;
        }

        public MinioClient GetMinioClient()
        {
            return minioClient;
        }

        public void SetMinioClient(MinioClient minioClient)
        {
            this.minioClient = minioClient;
        }
    }
}
