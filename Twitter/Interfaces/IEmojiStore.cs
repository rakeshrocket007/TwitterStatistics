using System.Collections.Generic;

namespace JH.Twitter
{
    public interface IEmojiStore
    {
        List<EmojiItem> items { get; }
    }
}