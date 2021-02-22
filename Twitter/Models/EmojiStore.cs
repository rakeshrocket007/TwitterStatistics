using JH.Twitter.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JH.Twitter
{
    public class EmojiStore : IEmojiStore
    {
        public List<EmojiItem> items { get; }
        private ILogger _log;

        public EmojiStore(ILogger log, string path)
        {
            try
            {
                _log = log;
                items = JsonConvert.DeserializeObject<List<EmojiItem>>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path)));
                var c = items.FindAll(i => i.unifiedCh == "-1").Count;
            }
            catch(IOException iex)
            {
                _log.Error("Error reading data from the file : " + iex.Message);
                throw;
            }
            catch(Exception ex)
            {
                _log.Error("Error while deserializing the data from emoji store. " + ex.Message);
            }
        }

    }
    
    public class EmojiItem
    {
        public string name { get; set; }
        public string unifiedCh { get; private set; }
        public string unified
        {
            get { return unifiedCh.ToString(); }
            set
            {
                try
                {
                    var utf32Number = Convert.ToInt32(value, 16);
                    unifiedCh = char.ConvertFromUtf32(utf32Number);
                }
                catch { unifiedCh = "-1"; }
            }
            
        }
        public string non_qualified { get; set; }
    }


}
