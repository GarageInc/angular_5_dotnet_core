using System.Collections.Generic;

namespace depot {
    class ModerationDtoComparer : IEqualityComparer<ModerationDto> {

        private readonly IProducerCodeService _producerCodeService;
        public ModerationDtoComparer (IProducerCodeService producerCodeService) {
            this._producerCodeService = producerCodeService;
        }

        public static int BERETTA_ID = 2;
        public static int BERETTA_RIHHAI_ID = 17;

        bool IEqualityComparer<ModerationDto>.Equals (ModerationDto x, ModerationDto y) {
            if (isBerettaRihhai (x, y) && this._producerCodeService.IsEqualCodes (x.ProducerCodeTrimmed, y.ProducerCodeTrimmed)) {
                return true;
            }
            /*
                        if (x.ProducerCode.Contains ("20094647") && y.ProducerCode.Contains ("20094647")) {
                            var b = 2;
                        }
             */

            return this._producerCodeService.IsEqualCodes (x.ProducerCodeTrimmed, y.ProducerCodeTrimmed) && x.ProducerId == y.ProducerId;
        }

        int IEqualityComparer<ModerationDto>.GetHashCode (ModerationDto obj) {
            var hash = this._producerCodeService.GetHashCode (obj.ProducerCodeTrimmed);
            return hash;
        }

        public bool isBerettaRihhai (ModerationDto x, ModerationDto y) {
            if (x.ProducerId == BERETTA_ID && y.ProducerId == BERETTA_RIHHAI_ID ||
                x.ProducerId == BERETTA_RIHHAI_ID && y.ProducerId == BERETTA_ID) {
                return true;
            } else {
                return false;
            }
        }
    }

}