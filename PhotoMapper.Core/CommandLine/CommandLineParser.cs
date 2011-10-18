using System;
using System.Collections.Generic;

namespace PhotoMapper.Core.CommandLine
{
    public class CommandLineArgs
    {
        private Dictionary<string, string> commandargs = new Dictionary<string, string>();

        public CommandLineArgs(Dictionary<string,string> mappings)
        {
            this.commandargs = mappings;
        }

        public static CommandLineArgs ParseCommandLine(string[] args)
        {
            Dictionary<string, string> mappings = new Dictionary<string, string>();

            foreach (string arg in args)
            {
                // Skip over command line
                if (!arg.StartsWith("/")) continue;

                string[] values = arg.Split(new char[] {':'},2);
                string key = values[0].TrimStart('/');

                if (key == "?" || values.Length == 1)
                {
                    mappings.Add(key, "");
                    continue;
                }

                mappings.Add(key, values[1]);
            }

            return new CommandLineArgs(mappings);
        }

        public string this[string argKey]
        {
            get
            {
                if (this.ContainsArg(argKey))
                {
                    return this.commandargs[argKey];
                }
                
                throw new ArgumentOutOfRangeException(String.Format("Arg with key {0} could not be found", argKey));
            }
        }

        public bool ContainsArg(string key)
        {
            return this.commandargs.ContainsKey(key);
        }

        public bool HasArgs {
            get
            { 
                return (this.commandargs.Count > 0);
            }
        }

        public delegate T Func<T>();

        public bool IsVaild(Func<bool> vaildcheck)
        {
            return vaildcheck.Invoke();
        }

        public string InDir 
        {
            get
            {
                return this["indir"];
            }
        }

        public string OutDir
        {
            get
            {
                return this["outdir"];
            }
        }

        public string OutName
        {
            get
            {
                return this["outname"];
            }
        }

        public bool Recursive
        {
            get
            {
                return this.ContainsArg("r");
            }
        }

        public ImageProcessor.FileFormat Format
        {
            get
            {
                if (this.ContainsArg("tab") && this.ContainsArg("mif"))
                    return ImageProcessor.FileFormat.MIF | ImageProcessor.FileFormat.TAB;
                
                if (this.ContainsArg("mif"))
                    return ImageProcessor.FileFormat.MIF;
                
                if (this.ContainsArg("tab"))
                    return ImageProcessor.FileFormat.TAB;
                
                return ImageProcessor.FileFormat.None;
            }
        }
    }
}
