using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace depot {
    public static class FileValidator {
        internal static void Validate (IFormFileCollection files, string[] v) {

            foreach (var f in files) {
                var anyMatched = v.Any (x => f.FileName.EndsWith (x));

                if (!anyMatched) {
                    throw new Exception ("Not valid format!");
                }
            }
        }
    }
}