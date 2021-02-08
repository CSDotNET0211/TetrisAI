using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GeneticAlgorithm
{
    /// <summary>
    /// 単位
    /// </summary>
    const float oneByte = 0.25f;

    /// <summary>
    /// 値を遺伝子文字列に変換します
    /// </summary>
    /// <param name="value">変換する値</param>
    /// <returns>遺伝子文字列</returns>
    static private string EncodeToGene(float value)
    {
        return Convert.ToString((int)(value / oneByte), 2).PadLeft(10, '0');

    }

    /// <summary>
    /// 遺伝子文字列を値に変換します
    /// </summary>
    /// <param name="gene">遺伝子文字列</param>
    /// <returns>変換された値</returns>
    static private float DecodeFromGene(string gene)
    {
        return (oneByte * Convert.ToInt32(gene, 2)) - 100;

    }

    /// <summary>
    /// 複数の値を遺伝子文字列に変換します
    /// </summary>
    /// <param name="values">値の配列</param>
    /// <returns>遺伝子文字列</returns>
    static public string CreateChromosome(float[] values)
    {
        string returnvalue = string.Empty;

        foreach (float value in values)
        {
            returnvalue += EncodeToGene(value);
        }

        return returnvalue;
    }

    /// <summary>
    /// 遺伝子文字列を値の配列に変換します
    /// </summary>
    /// <param name="chromosome">遺伝子文字列</param>
    /// <returns>値の配列</returns>
    static public float[] DecodeChromosome(string chromosome)
    {
        float[] returnValue = new float[chromosome.Length / 10];
        for (int i = 0; i < returnValue.Length; i++)
        {
            returnValue[i] = DecodeFromGene(chromosome.Substring(i * 10, 10));
        }

        return returnValue;
    }
    /// <summary>
    /// ２点交叉
    /// </summary>
    /// <param name="chromosome1">遺伝子文字列１</param>
    /// <param name="chromosome2">遺伝子文字列２</param>
    /// <returns></returns>
    static public string TwoPointCrossOver(string chromosome1, string chromosome2)
    {
        int pointMin = 1;
        int pointMax = chromosome1.Length - 2;

        Random rand = new Random();
        int point1 = rand.Next(pointMin, pointMax);
        int point2;
        do
        {
            point2 = rand.Next(pointMin, pointMax);
        } while (point1 == point2);

        if (point1 > point2)
        {
            int tempo = point1;
            point1 = point2;
            point2 = tempo;
        }

        int length = point2 - point1;

        StringBuilder sb = new StringBuilder(chromosome1);

        sb.Replace(chromosome1.Substring(point1, length), chromosome2.Substring(point1, length), point1, length);
        return sb.ToString();
    }
    /// <summary>
    /// 遺伝子文字列を一定確率で突然変異させます
    /// </summary>
    /// <param name="chromosome">遺伝子文字列</param>
    /// <returns>遺伝子文字列</returns>
    static public string Mutation(string chromosome)
    {
        Random rand = new Random();
        int random = rand.Next(0, chromosome.Length - 1);
        string value;
        string value1;
        if (chromosome[random] == '1')
        {
            value = "0";
            value1 = "1";

        }
        else
        {
            value = "1";
            value1 = "0";
        }


        StringBuilder sb = new StringBuilder(chromosome);
        sb.Replace(value1, value, random, 1);
        return sb.ToString();
    }
}

struct Chromosome
{
    public Chromosome(float eval, string chromosome)
    {
        this.eval = eval;
        this.chromosome = chromosome;
    }

    public float eval;
    public string chromosome;
}

class Probability
{
    /// <summary>
    /// 確率判定
    /// </summary>
    /// <param name="fPercent">確率 (0~100)</param>
    /// <returns>当選結果 [true]当選</returns>
    public static bool Percent(float fPercent)
    {
        Random random = new Random();

        float fProbabilityRate = random.Next(0, 100);

        if (fPercent == 100.0f && fProbabilityRate == fPercent)
        {
            return true;
        }
        else if (fProbabilityRate < fPercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
