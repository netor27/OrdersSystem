using System;
using System.Collections.Generic;
using System.Text;

using Gbm.Ordering.Domain.Common;


namespace Gbm.Ordering.Domain.AggregatesModel.BuyerAggregate
{
    public class CardType : Enumeration<CardType>
    {
        public static CardType Amex = new CardType(1, "Amex");
        public static CardType Visa = new CardType(2, "Visa");
        public static CardType MasterCard = new CardType(3, "MasterCard");

        protected CardType(int id, string name) : base(id, name) { }
    }
}
