namespace RamanSoftwareV2.Models
{
    public class SpectrumData   // 光谱数据
    {
        public double[] Raw { get; set; } = new double[0];
        public double[] Preprocessed { get; set; } = new double[0];
        public double[] Analyzed { get; set; } = new double[0];
    }
}
