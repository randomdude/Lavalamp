namespace WebService1
{
    // The webservice to build returns one of these.
    public class buildResults
    {
        public bool success;
        public string stdout;
        public errorFile[] errors;
        public string failReason;
    }

    // One of these is returned for each .err file generated
    public class errorFile
    {
        public string name;
        public string contents;
    }


}