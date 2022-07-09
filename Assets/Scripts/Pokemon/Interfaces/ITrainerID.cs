using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE {
    #region Trainer ID
    interface ITrainerID {
        int TID { get; set; }
        int SID { get; set; }
    }

    static partial class Extensions {
        public static bool IsShiny(this ITrainerID tr, uint pid) {
            var xor = tr.SID ^ tr.TID ^ (pid >> 16) ^ pid;
            return xor < 8;
        }
    }
    #endregion
    #region Height
    interface IScaledSize {
        byte WeightScalar { get; set; }
        byte HeightScalar { get; set; }
    }
    interface IScaledSizeAbsolute {
        float HeightAbsolute { get; set; }
        float WeightAbsolute { get; set; }
    }
    interface IScaledSizeValue : IScaledSize, IScaledSizeAbsolute {
        void ResetHeight();
        void ResetWeight();
        float CalcHeightAbsolute { get; }
        float CalcWeightAbsolute { get; }
    }
    #endregion
    #region Memory
    #region OT
    interface IMemoryOT {
        byte OT_Memory { get; set; }
        byte OT_Intensity { get; set; }
        byte OT_Feeling { get; set; }
        byte OT_TextVar { get; set; }
    }
    static partial class Extensions {
        static void ClearMemoriesOT(this IMemoryOT ot) {
            ot.OT_Feeling = ot.OT_TextVar = ot.OT_Memory = ot.OT_Intensity = 0;
        }
    }
    #endregion
    #region CT
    
    #endregion
    #endregion
}

