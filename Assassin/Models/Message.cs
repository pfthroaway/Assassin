using System;

namespace Assassin.Models
{
    /// <summary>Represents a <see cref="Message"/> sent to a <see cref="User"/>.</summary>
    public class Message
    {
        /// <summary>ID of the <see cref="Message"/>.</summary>
        public int ID { get; set; }

        /// <summary><see cref="User"/> who sent the <see cref="Message"/>.</summary>
        public string UserFrom { get; set; }

        /// <summary><see cref="User"/> to whom the <see cref="Message"/> is written.</summary>
        public string UserTo { get; set; }

        /// <summary>What the <see cref="Message"/> says.</summary>
        public string Contents { get; set; }

        /// <summary>Date the <see cref="Message"/> was sent.</summary>
        public DateTime DateSent { get; set; }

        /// <summary>Was this <see cref="Message"/> sent by a guild leader?</summary>
        public bool GuildMessage { get; set; }

        /// <summary>Constructs a new instance of <see cref="Message"/> by assigning property values.</summary>
        /// <param name="sentFrom"><see cref="User"/> who sent the <see cref="Message"/></param>
        /// <param name="sentTo"><see cref="User"/> to whom the <see cref="Message"/> is written</param>
        /// <param name="msg">What the <see cref="Message"/> says</param>
        /// <param name="sent">Date the <see cref="Message"/> was sent</param>
        /// <param name="guildMsg">Was this <see cref="Message"/> sent by a guild leader?</param>
        public Message(int msgID, string sentFrom, string sentTo, string msg, DateTime sent, bool guildMsg)
        {
            ID = msgID;
            UserFrom = sentFrom;
            UserTo = sentTo;
            Contents = msg;
            DateSent = sent;
            GuildMessage = guildMsg;
        }

        /// <summary>Constructs a new instance of <see cref="Message"/> by copying another <see cref="Message"/>.</summary>
        /// <param name="other"><see cref="Message"/> to be copied</param>
        public Message(Message other)
        {
            ID = other.ID;
            UserFrom = other.UserFrom;
            UserTo = other.UserTo;
            Contents = other.Contents;
            DateSent = other.DateSent;
            GuildMessage = other.GuildMessage;
        }
    }
}