using System;

namespace DotvvmWeather.Services
{
    public class BME280CalibrationData
    {
        //BME280 Registers
        public UInt16 dig_T1 { get; set; }
        public Int16 dig_T2 { get; set; }
        public Int16 dig_T3 { get; set; }

        public UInt16 dig_P1 { get; set; }
        public Int16 dig_P2 { get; set; }
        public Int16 dig_P3 { get; set; }
        public Int16 dig_P4 { get; set; }
        public Int16 dig_P5 { get; set; }
        public Int16 dig_P6 { get; set; }
        public Int16 dig_P7 { get; set; }
        public Int16 dig_P8 { get; set; }
        public Int16 dig_P9 { get; set; }

        public UInt16 dig_H1 { get; set; }
        public Int16 dig_H2 { get; set; }
        public Int16 dig_H3 { get; set; }
        public Int16 dig_H4 { get; set; }
        public Int16 dig_H5 { get; set; }
        public Int16 dig_H6 { get; set; }
    }
}