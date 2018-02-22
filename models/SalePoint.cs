using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace depot {
    // Склад
    [Table ("SalePoint")]
    public class SalePoint : ContactAbstract {

        public string Name { get; set; }
        public IList<TimeWork> TimeWorks { get; set; }
        public string DeliveryMethod { get; set; }

        internal void UpdateTimeWorks (SalePoint sp) {
            for (var i = 0; i < sp.TimeWorks.Count (); i++) {
                var newTimeWork = sp.TimeWorks[i];
                if (i < this.TimeWorks.Count) {
                    var oldTimeWork = this.TimeWorks[i];
                    oldTimeWork.Description = newTimeWork.Description;
                } else {
                    this.TimeWorks.Add (newTimeWork);
                }
            }
        }
    }

    [Table ("TimeWork")]
    public class TimeWork : BaseModelAbstract {
        public string Description { get; set; }

        // public SalePoint SalePoint { get; set; }
        // public int SalePointId { get; set; }
    }
}