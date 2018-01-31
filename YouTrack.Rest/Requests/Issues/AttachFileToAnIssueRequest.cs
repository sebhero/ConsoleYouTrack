using System;
using System.IO;

namespace YouTrack.Rest.Requests.Issues
{
    class AttachFileToAnIssueRequest : YouTrackRequest, IYouTrackPostWithFileRequest
    {
        private byte[] bytes;

        public AttachFileToAnIssueRequest(string issueId, string filePath, string group = null) : base(String.Format("/rest/issue/{0}/attachment", issueId))
        {
            FilePath = filePath;
            Name = "files";

            ResourceBuilder.AddParameter("name", Path.GetFileNameWithoutExtension(filePath));
            ResourceBuilder.AddParameter("group", group);
        }

        public AttachFileToAnIssueRequest(string issueId, string fileName, byte[] bytes, string group = null) : base(String.Format("/rest/issue/{0}/attachment", issueId))
        {
            Name = "files";
            this.bytes = bytes;
            FileName = fileName;

            ResourceBuilder.AddParameter("name", Path.GetFileNameWithoutExtension(fileName));
            ResourceBuilder.AddParameter("group", group);
        }

        public string FilePath { get; private set; }
        public string Name { get; private set; }
        public string FileName { get; private set; }

        public byte[] Bytes
        {
            get { return bytes; }
        }


        public bool HasBytes
        {
            get { return bytes != null; }
        }
    }
}
