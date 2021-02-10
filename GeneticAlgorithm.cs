using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class GeneticAlgorithm
{
    /// <summary>
    /// 単位
    /// </summary>
    const float oneByte = 0.25f;

    #region フレーム定義
    //ミノ移動
    const int MoveFrame = 2;
    const int Clear1Line = 5;
    const int Clear2Line = 10;
    const int Clear3Line = 10;
    const int Clear4Line = 15;
    const int Hold = 2;
    const int MinoSet = 5;





    #endregion

    /// <summary>
    /// 値を遺伝子文字列に変換します
    /// </summary>
    /// <param name="value">変換する値</param>
    /// <returns>遺伝子文字列</returns>
    static public string EncodeToGene(float value)
    {
        value += 100;
        return Convert.ToString((int)(value / oneByte), 2).PadLeft(10, '0');

    }

    /// <summary>
    /// 遺伝子文字列を値に変換します
    /// </summary>
    /// <param name="gene">遺伝子文字列</param>
    /// <returns>変換された値</returns>
    static public float DecodeFromGene(string gene)
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
    /// <returns>真ん中をスワップしたものが生成されます</returns>
    static public string TwoPointCrossOver(string chromosome1, string chromosome2)
    {
        int pointMin = 1;
        int pointMax = chromosome1.Length;

        Random rand = new Random();
        int point1 = rand.Next(pointMin, pointMax - 1);
        int point2;
        do
        {
            point2 = rand.Next(pointMin, pointMax - 1);
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
    /// 遺伝子文字列を突然変異させます
    /// </summary>
    /// <param name="chromosome">遺伝子文字列</param>
    /// <returns>遺伝子文字列</returns>
    static public string Mutation(string chromosome)
    {
        Random rand = new Random();
        int random = rand.Next(0, chromosome.Length);
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

    static public void Learn()
    {
        int threadCount;
        int threadCount2;
        ThreadPool.GetMaxThreads(out threadCount, out threadCount2);
        Console.WriteLine("スレッド数は"+threadCount+"です。");

        Console.WriteLine("世代数を入力してください。");
        int generationDestination = int.Parse(Console.ReadLine());
        Console.WriteLine("学習を開始します。");


        int nowGenerationIndex = 0;
        int index = 0;
        TetrisEnvironment.MinoKind[] nexts = new TetrisEnvironment.MinoKind[5];
        List<TetrisEnvironment.MinoKind> _leftMinoBag = new List<TetrisEnvironment.MinoKind>();
        Chromosome[] nowChromosome = new Chromosome[generationDestination];
        const int FrameMax = 10800;

        int[,] field = new int[10, 25];

        Random random = new Random();
        while (true)
        {
            for (int i = 0; i < generationDestination; i++)
            {
                float[] values = new float[11];
                values[0] = random.Next(-100, 155);
                values[1] = random.Next(-100, 155);
                values[2] = random.Next(-100, 155);
                values[3] = random.Next(-100, 155);
                values[4] = random.Next(-100, 155);
                values[5] = random.Next(-100, 155);
                values[6] = random.Next(-100, 155);
                values[7] = random.Next(-100, 155);
                values[8] = random.Next(-100, 155);
                values[9] = random.Next(-100, 155);
                values[10] = random.Next(-100, 155);

                nowChromosome[i].chromosome = CreateChromosome(values);
            }


        Label:
            var values2 = DecodeChromosome(nowChromosome[index].chromosome);

            NewEvaluation.h_sumofheight = values2[0];
            NewEvaluation.h_dekoboko = values2[1];
            NewEvaluation.h_holeCount = values2[2];
            NewEvaluation.h_holeup1 = values2[3];
            NewEvaluation.h_holeup2 = values2[4];
            NewEvaluation.h_holeup3 = values2[5];
            NewEvaluation.h_holeup4 = values2[6];
            NewEvaluation.h_line1 = values2[7];
            NewEvaluation.h_line2 = values2[8];
            NewEvaluation.h_line3 = values2[9];
            NewEvaluation.h_line4 = values2[10];

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 25; y++)
                {
                    field[x, y] = 0;
                }
            }
            Console.Clear();
            Console.WriteLine($"{nowGenerationIndex}世代　{index}/{generationDestination}\r\n\r\n");

            Console.WriteLine($"現在の評価\r\n" +
            $"高さ合計:{ NewEvaluation.h_sumofheight}\r\n" +
            $"1ライン消し:{ NewEvaluation.h_line1}\r\n" +
            $"2ライン消し:{ NewEvaluation.h_line2}\r\n" +
            $"3ライン消し:{ NewEvaluation.h_line3}\r\n" +
            $"4ライン消し:{ NewEvaluation.h_line4}\r\n" +
            $"でこぼこ:{ NewEvaluation.h_dekoboko}\r\n" +
            $"穴:{ NewEvaluation.h_holeCount}\r\n" +
            $"穴の上ミノ1:{ NewEvaluation.h_holeup1}\r\n" +
            $"穴の上ミノ2:{ NewEvaluation.h_holeup2}\r\n" +
            $"穴の上ミノ3:{ NewEvaluation.h_holeup3}\r\n" +
            $"穴の上ミノ4:{ NewEvaluation.h_holeup4}\r\n");

            TetrisEnvironment.UpdateNexts(nexts, _leftMinoBag);
            TetrisEnvironment.UpdateNexts(nexts, _leftMinoBag);
            TetrisEnvironment.UpdateNexts(nexts, _leftMinoBag);
            TetrisEnvironment.UpdateNexts(nexts, _leftMinoBag);
            TetrisEnvironment.UpdateNexts(nexts, _leftMinoBag);
            var minotemp = TetrisEnvironment.UpdateNexts(nexts, _leftMinoBag);
            TetrisEnvironment.Mino nowMino = TetrisEnvironment.CreateMino(minotemp);
            bool canhold = true;
            TetrisEnvironment.MinoKind? hold = new TetrisEnvironment.MinoKind?();
            int frameCount = 0;
            int score = 0;
            int ren = 0;
            bool backtoback = false;

            while (true)
            {
                var way = NewTetrisAI.Find(field, (int)nowMino.kind, Utility.Enum2IntArray(new TetrisEnvironment.MinoKind[1] { nexts[0] }), 0, (int?)hold, canhold);

                way.way = way.way.Substring(0, way.way.IndexOf('5') + 1);
                foreach (char action in way.way)
                {


                    switch (int.Parse(action.ToString()))
                    {
                        case (int)TetrisEnvironment.Move.SoftDrop:
                            if (TetrisEnvironment.CheckValidPosition(field, nowMino.positions, new TetrisEnvironment.Vector2(0, -1)))
                            {
                                nowMino.positions[0] += new TetrisEnvironment.Vector2(0, -1);
                                nowMino.positions[1] += new TetrisEnvironment.Vector2(0, -1);
                                nowMino.positions[2] += new TetrisEnvironment.Vector2(0, -1);
                                nowMino.positions[3] += new TetrisEnvironment.Vector2(0, -1);

                                frameCount += MoveFrame;
                            }
                            break;

                        case (int)TetrisEnvironment.Move.HardDrop:
                            frameCount += MinoSet;
                            canhold = true;
                            int count = 0;
                            while (true)
                            {
                                if (TetrisEnvironment.CheckValidPosition(field, nowMino.positions, new TetrisEnvironment.Vector2(0, count)))
                                    count--;
                                else
                                { count++; break; }
                            }

                            nowMino.positions[0] += new TetrisEnvironment.Vector2(0, count);
                            nowMino.positions[1] += new TetrisEnvironment.Vector2(0, count);
                            nowMino.positions[2] += new TetrisEnvironment.Vector2(0, count);
                            nowMino.positions[3] += new TetrisEnvironment.Vector2(0, count);

                            foreach (var position in nowMino.positions)
                            {
                                field[position.x, position.y] = 1;
                            }

                            //ミノ消去ANDスコア加算
                            int clearedCount = TetrisEnvironment.CheckLine(field);

                            if (clearedCount == 4)
                                backtoback = true;
                            else
                                backtoback = false;

                            if (clearedCount == 0)
                                ren = 0;
                            else
                                ren++;
                            TetrisEnvironment.UpdateScore(clearedCount, ren, ref score, backtoback);

                            var temp2 = TetrisEnvironment.UpdateNexts(nexts, _leftMinoBag);
                            nowMino = TetrisEnvironment.CreateMino(temp2);

                            if (!TetrisEnvironment.CheckValidPosition(field, nowMino.positions, TetrisEnvironment.Vector2.Zero) || frameCount >= FrameMax)
                            {
                                //死亡


                                if (index == generationDestination - 1)
                                {//次の世代に移行



                                    List<Chromosome> newChromosomes = new List<Chromosome>();
                                    List<Chromosome> tempList = new List<Chromosome>();

                                    nowChromosome[index].eval = score;


                                    tempList.AddRange(nowChromosome);

                                    tempList.Sort((a, b) => b.eval.CompareTo(a.eval));
                                    score = 0;

                                    //エリート方式で追加
                                    for (int i = 0; i < generationDestination - 3; i++)
                                        newChromosomes.Add(new Chromosome(0, tempList[i].chromosome));

                                    newChromosomes.Add(new Chromosome(0, GeneticAlgorithm.TwoPointCrossOver(tempList[0].chromosome, tempList[1].chromosome)));
                                    newChromosomes.Add(new Chromosome(0, GeneticAlgorithm.TwoPointCrossOver(tempList[0].chromosome, tempList[2].chromosome)));
                                    if (Probability.Percent(3))
                                        newChromosomes.Add(new Chromosome(0, GeneticAlgorithm.Mutation(tempList[0].chromosome)));
                                    else
                                        newChromosomes.Add(new Chromosome(0, tempList[0].chromosome));


                                    nowChromosome = newChromosomes.ToArray();
                                    nowGenerationIndex++;
                                    index = 0;
                                }
                                else nowChromosome[index].eval = score;


                                index++;
                                goto Label;
                            }
                            break;

                        case (int)TetrisEnvironment.Move.Hold:
                            if (canhold)
                            {
                                canhold = false;
                                if (hold == null)
                                {
                                    hold = nowMino.kind;

                                    var temp = TetrisEnvironment.UpdateNexts(nexts, _leftMinoBag);
                                    nowMino = TetrisEnvironment.CreateMino(temp);
                                }
                                else
                                {
                                    var temp = nowMino.kind;
                                    nowMino = TetrisEnvironment.CreateMino((TetrisEnvironment.MinoKind)hold);
                                    hold = temp;
                                }
                            }
                            break;

                        case (int)TetrisEnvironment.Move.Left:
                            if (TetrisEnvironment.CheckValidPosition(field, nowMino.positions, TetrisEnvironment.Vector2.MinusX1))
                            {
                                nowMino.positions[0] += TetrisEnvironment.Vector2.MinusX1;
                                nowMino.positions[1] += TetrisEnvironment.Vector2.MinusX1;
                                nowMino.positions[2] += TetrisEnvironment.Vector2.MinusX1;
                                nowMino.positions[3] += TetrisEnvironment.Vector2.MinusX1;

                                frameCount += MoveFrame;
                            }
                            break;

                        case (int)TetrisEnvironment.Move.LeftRotate:
                            if (TetrisEnvironment.RotateMino(TetrisEnvironment.Move.LeftRotate, ref nowMino, field))
                            {
                                frameCount += MoveFrame;
                            }
                            break;

                        case (int)TetrisEnvironment.Move.Right:
                            if (TetrisEnvironment.CheckValidPosition(field, nowMino.positions, TetrisEnvironment.Vector2.X1))
                            {
                                nowMino.positions[0] += TetrisEnvironment.Vector2.X1;
                                nowMino.positions[1] += TetrisEnvironment.Vector2.X1;
                                nowMino.positions[2] += TetrisEnvironment.Vector2.X1;
                                nowMino.positions[3] += TetrisEnvironment.Vector2.X1;

                                frameCount += MoveFrame;
                            }
                            break;

                        case (int)TetrisEnvironment.Move.RightRotate:
                            if (TetrisEnvironment.RotateMino(TetrisEnvironment.Move.RightRotate, ref nowMino, field))
                            {
                                frameCount += MoveFrame;
                            }

                            break;
                    }

                    //TetrisEnvironment.Print(field, nowMino.positions, false);
                    //Console.WriteLine("現在のミノ");
                    //Console.Write(nowMino.kind + "\r\n");
                    //Console.WriteLine("ネクスト");
                    //Console.Write(nexts[0]);
                    //Console.Write(nexts[1]);
                    //Console.Write(nexts[2]);
                    //Console.Write(nexts[3]);
                    //Console.Write(nexts[4] + "\r\n");
                    //Console.WriteLine("ホールド");
                    //Console.WriteLine(hold + "\r\n");
                    //Thread.Sleep(10);
                    ////Console.ReadKey();
                }

            }
        }

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
