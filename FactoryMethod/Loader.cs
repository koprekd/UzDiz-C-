using System;
using System.Collections.Generic;
using System.Text;

namespace dkoprek_zadaca_3
{
    public interface Loader
    {
        List<Object> LoadDataFromFile(string[] args);
    }
}
