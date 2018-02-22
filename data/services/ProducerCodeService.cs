using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Hosting;

namespace depot {

    public interface IProducerCodeService {
        bool IsEqualCodes (string one, string with);
        int GetHashCode (string producerCode);
        string TrimCode (string producerCode);
    }

    public class ProducerCodeService : IProducerCodeService {

        private readonly char STOP_ZERO = '0';

        public bool IsEqualCodes (string one, string second) {
            return one == second
            /*||
                (one.StartsWith (STOP_ZERO) && one.TrimStart (STOP_ZERO[0]) == second) ||
                (second.StartsWith (STOP_ZERO) && second.TrimStart (STOP_ZERO[0]) == one)*/
            ;
        }

        public int GetHashCode (string producerCode) {
            return producerCode.GetHashCode ();
        }

        public string TrimCode (string producerCode) {
            var result = producerCode;

            if (result.Length > 4 && result.Any (x => x != '0')) {
                result = result.TrimStart (STOP_ZERO);
            }

            result = result.Replace ("-", "");
            result = result.Replace (".", "");

            return result.Length > 0 ? result : producerCode;
        }
    }

}