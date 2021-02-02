using System;
using System.Collections.Generic;
using System.Text;

namespace dkoprek_zadaca_3
{
    abstract class LoaderFactory
    {
        public abstract Loader CreateLoader();       

        public List<Object> GetObjects(string[] args)
        {
            var loader = CreateLoader();
            return loader.LoadDataFromFile(args);
        }
    }
}
