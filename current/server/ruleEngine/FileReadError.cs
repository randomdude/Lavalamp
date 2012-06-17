using System;

namespace ruleEngine
{
    public class fileReadError : Exception
    {
        private readonly string _file;

        public fileReadError(string filename)
        {
            this._file = filename;
        }

        public override string Message
        {
            get
            {
                return "could not load file '" + _file + "'";
            }
        }
    }
}
