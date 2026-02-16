using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMOffice.Services
{
    public class CableData
    {
        public string Size { get; set; }
        public double RatingAir { get; set; }
        public double RatingGround { get; set; }
        public double Impedance { get; set; } // mOhm/m or similar? Usually Ohm/km. Let's assume Ohm/km given the values (14.48, 8.87...).
    }

    public class CableSizingService
    {
        // Engineering Spec Constants
        private const double VoltageDropLimitPhase1 = 3.0; // %
        private const double VoltageDropLimitPhase3 = 5.0; // %
        private const double SafetyFactor = 1.15;

        // Cable Data Table (Copper Cables)
        private static readonly List<CableData> CopperCables = new List<CableData>
        {
            new CableData { Size = "1.5mm²", RatingAir = 19, RatingGround = 24, Impedance = 14.48 },
            new CableData { Size = "2.5mm²", RatingAir = 26, RatingGround = 32, Impedance = 8.87 },
            new CableData { Size = "4mm²", RatingAir = 35, RatingGround = 42, Impedance = 5.52 },
            new CableData { Size = "6mm²", RatingAir = 45, RatingGround = 53, Impedance = 3.69 },
            new CableData { Size = "10mm²", RatingAir = 62, RatingGround = 70, Impedance = 2.19 },
            new CableData { Size = "16mm²", RatingAir = 83, RatingGround = 91, Impedance = 1.38 }
        };

        /// <summary>
        /// Calculates the required cable size based on load, voltage, and length.
        /// </summary>
        /// <param name="loadAmps">The load current in Amperes.</param>
        /// <param name="voltage">The system voltage (e.g., 230, 400).</param>
        /// <param name="lengthMeters">The length of the cable run in meters.</param>
        /// <param name="isThreePhase">True for 3-phase, False for 1-phase (default false).</param>
        /// <param name="isGround">True if installed in ground, False for air (default false).</param>
        /// <returns>The recommended cable size string, or error message.</returns>
        public string CalculateCableSize(double loadAmps, double voltage, double lengthMeters, bool isThreePhase = false, bool isGround = false)
        {
            try
            {
                // 1. Apply Safety Factor to Load
                double designCurrent = loadAmps * SafetyFactor;

                // 2. Determine allowed Voltage Drop (%)
                double maxDropPercent = isThreePhase ? VoltageDropLimitPhase3 : VoltageDropLimitPhase1;
                double maxDropVolts = voltage * (maxDropPercent / 100.0);

                // 3. Select Cable
                // We need a cable that:
                // a) Has a current rating >= designCurrent
                // b) Has a voltage drop <= maxDropVolts
                
                // Voltage Drop Calculation Formula (Simplified approximation):
                // VD = (I * L * Z) / 1000 
                // Where Z is impedance in Ohm/km (mOhm/m), L is length in meters.
                // Note: The factor 'sqrt(3)' is needed for 3-phase voltage drop if using line-to-line voltage?
                // Standard approx: 
                // 1-phase: VD = 2 * I * L * (R cos phi + X sin phi) ... let's use Z directly as impedance scalar.
                // VD (Volts) = (I * L * (mV/A/m)) / 1000 ... wait, impedance is likely Ohm/km.
                // Let's assume the provided Impedance is Ohm/km.
                // Drop (V) = I * (L/1000) * Z
                // For 1-phase loop, usually 2x length factor applies for resistance? 
                // Let's stick to a simple model given the spec brevity: V_drop = I * R_total.
                
                // Correct simplified approach for this exercise:
                // Drop = Current * (Impedance * Length / 1000) 
                // (Adjusting for 1-phase return path if impedance is single core? Usually tables give 'voltage drop per amp per meter' directly or Z per km).
                // Let's assume Impedance given is Ohm/km for the conductor.
                // 1-Phase Drop = 2 * I * (L/1000) * Z
                // 3-Phase Drop = sqrt(3) * I * (L/1000) * Z

                foreach (var cable in CopperCables)
                {
                    // Check Ampacity
                    double rating = isGround ? cable.RatingGround : cable.RatingAir;
                    if (rating < designCurrent)
                    {
                        continue; // Cable too small for current
                    }

                    // Check Voltage Drop
                    double dropVolts = 0.0;
                    if (isThreePhase)
                    {
                        dropVolts = Math.Sqrt(3) * designCurrent * (lengthMeters / 1000.0) * cable.Impedance;
                    }
                    else
                    {
                        dropVolts = 2.0 * designCurrent * (lengthMeters / 1000.0) * cable.Impedance;
                    }

                    if (dropVolts <= maxDropVolts)
                    {
                        return cable.Size; // Valid cable found
                    }
                }

                return "Load too high or run too long for available cables.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
