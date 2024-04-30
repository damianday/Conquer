using System;
using System.Collections.Generic;
using System.Drawing;

//namespace Library;

public static class Compute
{
    public static readonly DateTime SystemTime;

    public static int 扩展背包(int 扩展次数, int 当前消耗 = 0, int 当前位置 = 1, int 累计消耗 = 0)
    {
        if (当前位置 > 扩展次数)
        {
            return 累计消耗;
        }
        if (当前位置 <= 1)
        {
            int num = 累计消耗;
            当前消耗 = 2000;
            累计消耗 = num + 2000;
        }
        else if (当前位置 <= 16)
        {
            累计消耗 += (当前消耗 += 1000);
        }
        else if (当前位置 == 17)
        {
            int num2 = 累计消耗;
            当前消耗 = 20000;
            累计消耗 = num2 + 20000;
        }
        else
        {
            累计消耗 += (当前消耗 += 10000);
        }
        return 扩展背包(扩展次数, 当前消耗, 当前位置 + 1, 累计消耗);
    }

    public static int 扩展仓库(int 扩展次数, int 当前消耗 = 0, int 当前位置 = 1, int 累计消耗 = 0)
    {
        if (当前位置 > 扩展次数)
        {
            return 累计消耗;
        }
        if (当前位置 <= 1)
        {
            int num = 累计消耗;
            当前消耗 = 2000;
            累计消耗 = num + 2000;
        }
        else if (当前位置 <= 24)
        {
            累计消耗 += (当前消耗 += 1000);
        }
        else if (当前位置 == 25)
        {
            int num2 = 累计消耗;
            当前消耗 = 30000;
            累计消耗 = num2 + 30000;
        }
        else
        {
            累计消耗 += (当前消耗 += 10000);
        }
        return 扩展仓库(扩展次数, 当前消耗, 当前位置 + 1, 累计消耗);
    }

    public static int 扩展资源背包(int 扩展次数, int 当前消耗 = 0, int 当前位置 = 1, int 累计消耗 = 0)
    {
        if (当前位置 > 扩展次数)
        {
            return 累计消耗;
        }
        if (当前位置 <= 1)
        {
            int num = 累计消耗;
            当前消耗 = 10000;
            累计消耗 = num + 10000;
        }
        else if (当前位置 <= 24)
        {
            累计消耗 += (当前消耗 += 10000);
        }
        else if (当前位置 == 25)
        {
            int num2 = 累计消耗;
            当前消耗 = 10000;
            累计消耗 = num2 + 10000;
        }
        else
        {
            累计消耗 += (当前消耗 += 10000);
        }
        return 扩展资源背包(扩展次数, 当前消耗, 当前位置 + 1, 累计消耗);
    }

    public static int Clamp(int minimum, int value, int maximum) => Math.Clamp(value, minimum, maximum);

    public static GameDirection TurnAround(GameDirection direction, int RotationVector)
    {
        return direction + RotationVector % 8 * 1024 + 0;
    }

    public static int GetDistance(Point start, Point end)
    {
        int x = Math.Abs(end.X - start.X);
        int y = Math.Abs(end.Y - start.Y);
        return Math.Max(x, y);
    }

    public static bool InRange(Point start, Point end, int range) => GetDistance(start, end) <= range;

    public static bool InRange(Point start, Point end, Point range)
    {
        int x = Math.Abs(end.X - start.X);
        int y = Math.Abs(end.Y - start.Y);
        return x <= range.X && y <= range.Y;
    }

    public static int TimeSeconds(DateTime date)
    {
        return (int)(date - SystemTime).TotalSeconds;
    }

    public static bool 日期同周(DateTime 日期一, DateTime 日期二)
    {
        DateTime dateTime = ((日期二 > 日期一) ? 日期二 : 日期一);
        DateTime dateTime2 = ((日期二 > 日期一) ? 日期一 : 日期二);
        if ((dateTime - dateTime2).TotalDays > 7.0)
            return false;
        int num = Convert.ToInt32(dateTime.DayOfWeek);
        if (num == 0)
            num = 7;
        int num2 = Convert.ToInt32(dateTime2.DayOfWeek);
        if (num2 == 0)
            num2 = 7;
        if (num2 > num)
            return false;
        return true;
    }

    public static float CalculateLevelRatio(int playerLevel, int monsterLevel, ushort 减收益等级差 = 0, decimal 收益减少比率 = 1)
    {
        decimal val = (decimal)Math.Max(0, playerLevel - monsterLevel - 减收益等级差) * 收益减少比率;
        return (float)Math.Max(0m, val);
    }

    public static bool CalculateProbability(float probability)
    {
        if (probability >= 1f) return true;
        if (probability <= 0f) return false;

        return probability * 100_000_000f > (float)Random.Shared.Next(100_000_000);
    }

    public static Point GetPositionAround(Point point, int distance)
    {
        if (--distance >= 0)
        {
            int num = (int)Math.Floor(Math.Sqrt((double)distance + 0.25) - 0.5);
            int num2 = num * (num + 1);
            int num3 = num2 + num + 1;
            int num4 = ((num & 1) << 1) - 1;
            int x = num4 * (num + 1 >> 1);
            int y = x;
            if (distance < num3)
            {
                x -= num4 * (distance - num2 + 1);
            }
            else
            {
                x -= num4 * (num + 1);
                y -= num4 * (distance - num3 + 1);
            }
            return new Point(point.X + x, point.Y + y);
        }
        return point;
    }

    public static Point GetFrontPosition(Point start, Point end, int distance)
    {
        if (start == end)
            return start;

        float num = (float)distance / (float)GetDistance(start, end);
        int num2 = (int)Math.Round((float)(end.X - start.X) * num);
        int num3 = (int)Math.Round((float)(end.Y - start.Y) * num);
        return new Point(start.X + num2, start.Y + num3);
    }

    public static Point GetNextPosition(Point point, GameDirection dir, int distance)
    {
        Point result = dir switch
        {
            GameDirection.Up => new Point(point.X, point.Y + distance),
            GameDirection.UpLeft => new Point(point.X + distance, point.Y + distance),
            GameDirection.Left => new Point(point.X + distance, point.Y),
            GameDirection.Right => new Point(point.X - distance, point.Y),
            GameDirection.UpRight => new Point(point.X - distance, point.Y + distance),
            GameDirection.Down => new Point(point.X, point.Y - distance),
            GameDirection.DownRight => new Point(point.X - distance, point.Y - distance),
            GameDirection.DownLeft => new Point(point.X + distance, point.Y - distance),
        };
        return result;
    }

    public static GameDirection RandomDirection()
    {
        return (GameDirection)(Random.Shared.Next(8) * 1024);
    }

    public static GameDirection DirectionFromPoint(Point start, Point end)
    {
        int num = end.X - start.X;
        return (GameDirection)((int)(Math.Round((Math.Atan2(end.Y - start.Y, num) * 180.0 / Math.PI + 360.0) % 360.0 / 45.0) * 1024.0) % 8192);
    }

    public static GameDirection 正向方向(Point start, Point end)
    {
        if (start == end)
            return GameDirection.Left;

        var dir = DirectionFromPoint(end, start);
        var distance = Math.Max(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y)) - 1;
        var location = GetNextPosition(end, dir, distance);
        return DirectionFromPoint(start, location);
    }

    public static GameDirection RotateDirection(GameDirection dir, int rotation)
    {
        return (GameDirection)((int)(dir + rotation % 8 * 1024 + 8192) % 8192);
    }

    public static Point 点阵坐标转协议坐标(Point location)
    {
        return new Point(location.X * 32 - 16, location.Y * 32 - 16);
    }

    public static Point 协议坐标转点阵坐标(Point location)
    {
        return new Point((int)Math.Round((location.X + 16f) / 32f), (int)Math.Round((location.Y + 16f) / 32f));
    }

    public static Point 游戏坐标转点阵坐标(PointF 游戏坐标)
    {
        PointF pointF = default(PointF);
        pointF.Y = (游戏坐标.X + 游戏坐标.Y) / 0.707107f / 0.000976562f / 2f / 4096f;
        pointF.X = (游戏坐标.X / 0.707107f / 0.000976562f + 134217728f) / 4096f - pointF.Y;
        return new Point((int)((double)(pointF.X / 32f) + 0.5), (int)((double)(pointF.Y / 32f) + 0.5));
    }

    public static PointF 点阵坐标转游戏坐标(Point 点阵坐标)
    {
        PointF pointF = new PointF(((float)点阵坐标.X - 0.5f) * 32f, ((float)点阵坐标.Y - 0.5f) * 32f);
        PointF result = default(PointF);
        result.X = ((pointF.Y + pointF.X) * 4096f - 134217728f) * 0.707107f * 0.000976562f;
        result.Y = ((pointF.Y - pointF.X) * 4096f + 134217728f) * 0.707107f * 0.000976562f;
        return result;
    }

    public static int CalcAttackSpeed(int speed)
    {
        return Math.Clamp(speed, -50, 50) * 50;
    }

    public static float CalcLuck(int luck)
    {
        switch (luck)
        {
            case 0: return 0.1f;
            case 1: return 0.11f;
            case 2: return 0.13f;
            case 3: return 0.14f;
            case 4: return 0.17f;
            case 5: return 0.2f;
            case 6: return 0.25f;
            case 7: return 0.33f;
            case 8: return 0.5f;

            default:
                {
                    if (luck >= 9)
                        return 1f;
                    return 0f;
                }
        }
    }

    public static int CalculateAttack(int min, int max, int luck)
    {
        int n = ((luck >= 0) ? max : min);
        if (CalculateProbability(CalcLuck(Math.Abs(luck))))
        {
            double added = 0.0;
            // TODO: Config..
            /*if (luck <= Config.幸运额外1值 && luck > 9 && Config.CurrentVersion >= 1)
            {
                added += (double)Config.幸运额外1伤害;
            }
            else if (luck <= Config.幸运额外2值 && luck > Config.幸运额外1值 && Config.CurrentVersion >= 1)
            {
                added += (double)Config.幸运额外2伤害;
            }
            else if (luck <= Config.幸运额外3值 && luck > Config.幸运额外2值 && Config.CurrentVersion >= 1)
            {
                added += (double)Config.幸运额外3伤害;
            }
            else if (luck <= Config.幸运额外4值 && luck > Config.幸运额外3值 && Config.CurrentVersion >= 1)
            {
                added += (double)Config.幸运额外4伤害;
            }
            else if (luck <= Config.幸运额外5值 && luck > Config.幸运额外4值 && Config.CurrentVersion >= 1)
            {
                added += (double)Config.幸运额外5伤害;
            }*/
            return n + n * (int)(added * 10.0) / 10;
        }
        return Random.Shared.Next(Math.Min(min, max), Math.Max(min, max) + 1);
    }

    public static int CalculateDefence(int 下限, int 上限)
    {
        if (上限 >= 下限)
        {
            return Random.Shared.Next(下限, 上限 + 1);
        }
        return Random.Shared.Next(上限, 下限 + 1);
    }

    public static bool 直线方向(Point 原点, Point 锚点)
    {
        int num = 原点.X - 锚点.X;
        int num2 = 原点.Y - 锚点.Y;
        if (num != 0 && num2 != 0)
        {
            return Math.Abs(num) == Math.Abs(num2);
        }
        return true;
    }

    public static bool CalculateHit(float accuracy, float agility, float hit, float evade)
    {
        float probability = ((agility == 0f) ? 1f : (accuracy / agility));
        float n = hit - evade;

        if (n == 0f)
            return CalculateProbability(probability);
        
        if (n >= 0f)
        {
            if (!CalculateProbability(probability))
                return CalculateProbability(n);
            return true;
        }

        if (CalculateProbability(probability))
            return !CalculateProbability(0f - n);

        return false;
    }

    public static Point[] CalculateGrid(Point point, GameDirection direction, ObjectSize range)
    {
        Point[] grid;
        switch (range)
        {
            case ObjectSize.Single1x1:
                grid = new Point[1] { point };
                break;
            case ObjectSize.HalfMoon3x1:
                {
                    Point[] array = direction switch
                    {
                        GameDirection.Up => new Point[5]
                        {
                    point,
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X - 1, point.Y - 1)
                        },
                        GameDirection.UpLeft => new Point[5]
                        {
                    point,
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y - 2),
                    new Point(point.X - 2, point.Y)
                        },
                        GameDirection.Left => new Point[5]
                        {
                    point,
                    new Point(point.X, point.Y - 1),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X - 1, point.Y + 1)
                        },
                        GameDirection.Right => new Point[5]
                        {
                    point,
                    new Point(point.X, point.Y + 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X + 1, point.Y - 1)
                        },
                        GameDirection.UpRight => new Point[5]
                        {
                    point,
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X + 2, point.Y),
                    new Point(point.X, point.Y - 2)
                        },
                        GameDirection.DownLeft => new Point[5]
                        {
                    point,
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X - 2, point.Y),
                    new Point(point.X, point.Y + 2)
                        },
                        GameDirection.Down => new Point[5]
                        {
                    point,
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X + 1, point.Y + 1)
                        },
                        _ => new Point[5]
                        {
                    point,
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y + 2),
                    new Point(point.X + 2, point.Y)
                        },
                    };

                    grid = array;
                    break;
                }
            case ObjectSize.HalfMoon3x2:
                {
                    Point[] array = direction switch
                    {
                        GameDirection.Up => new Point[8]
                        {
                    point,
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X - 1, point.Y + 1)
                        },
                        GameDirection.UpLeft => new Point[8]
                        {
                    point,
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X - 1, point.Y + 1)
                        },
                        GameDirection.Left => new Point[8]
                        {
                    point,
                    new Point(point.X, point.Y - 1),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X + 1, point.Y + 1)
                        },
                        GameDirection.Right => new Point[8]
                        {
                    point,
                    new Point(point.X, point.Y + 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X - 1, point.Y - 1)
                        },
                        GameDirection.UpRight => new Point[8]
                        {
                    point,
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X - 1, point.Y - 1)
                        },
                        GameDirection.DownLeft => new Point[8]
                        {
                    point,
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X + 1, point.Y + 1)
                        },
                        GameDirection.Down => new Point[8]
                        {
                    point,
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X + 1, point.Y - 1)
                        },
                        _ => new Point[8]
                        {
                    point,
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X + 1, point.Y - 1)
                        },
                    };
                    grid = array;
                    break;
                }
            case ObjectSize.HalfMoon3x3:
                {
                    Point[] array = direction switch
                    {
                        GameDirection.Up => new Point[12]
                        {
                    point,
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X + 2, point.Y),
                    new Point(point.X - 2, point.Y),
                    new Point(point.X + 2, point.Y - 1),
                    new Point(point.X - 2, point.Y - 1)
                        },
                        GameDirection.UpLeft => new Point[12]
                        {
                    point,
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y - 2),
                    new Point(point.X - 2, point.Y),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X + 1, point.Y - 2),
                    new Point(point.X - 2, point.Y + 1)
                        },
                        GameDirection.Left => new Point[12]
                        {
                    point,
                    new Point(point.X, point.Y - 1),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X, point.Y - 2),
                    new Point(point.X, point.Y + 2),
                    new Point(point.X - 1, point.Y - 2),
                    new Point(point.X - 1, point.Y + 2)
                        },
                        GameDirection.Right => new Point[12]
                        {
                    point,
                    new Point(point.X, point.Y + 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X, point.Y + 2),
                    new Point(point.X, point.Y - 2),
                    new Point(point.X + 1, point.Y + 2),
                    new Point(point.X + 1, point.Y - 2)
                        },
                        GameDirection.UpRight => new Point[12]
                        {
                    point,
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X + 2, point.Y),
                    new Point(point.X, point.Y - 2),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X + 2, point.Y + 1),
                    new Point(point.X - 1, point.Y - 2)
                        },
                        GameDirection.DownLeft => new Point[12]
                        {
                    point,
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X - 2, point.Y),
                    new Point(point.X, point.Y + 2),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X - 2, point.Y - 1),
                    new Point(point.X + 1, point.Y + 2)
                        },
                        GameDirection.Down => new Point[12]
                        {
                    point,
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X - 2, point.Y),
                    new Point(point.X + 2, point.Y),
                    new Point(point.X - 2, point.Y + 1),
                    new Point(point.X + 2, point.Y + 1)
                        },
                        _ => new Point[12]
                        {
                    point,
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y + 2),
                    new Point(point.X + 2, point.Y),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X - 1, point.Y + 2),
                    new Point(point.X + 2, point.Y - 1)
                        },
                    };
                    grid = array;
                    break;
                }
            case ObjectSize.Hollow3x3:
                grid = new Point[8]
                {
                GetNextPosition(point, GameDirection.Up, 1),
                GetNextPosition(point, GameDirection.Down, 1),
                GetNextPosition(point, GameDirection.Left, 1),
                GetNextPosition(point, GameDirection.Right, 1),
                GetNextPosition(point, GameDirection.UpLeft, 1),
                GetNextPosition(point, GameDirection.DownLeft, 1),
                GetNextPosition(point, GameDirection.UpRight, 1),
                GetNextPosition(point, GameDirection.DownRight, 1)
                };
                break;
            case ObjectSize.Solid3x3:
                grid = new Point[9]
                {
                point,
                GetNextPosition(point, GameDirection.Up, 1),
                GetNextPosition(point, GameDirection.Down, 1),
                GetNextPosition(point, GameDirection.Left, 1),
                GetNextPosition(point, GameDirection.Right, 1),
                GetNextPosition(point, GameDirection.UpLeft, 1),
                GetNextPosition(point, GameDirection.DownLeft, 1),
                GetNextPosition(point, GameDirection.UpRight, 1),
                GetNextPosition(point, GameDirection.DownRight, 1)
                };
                break;
            case ObjectSize.Solid5x5:
                grid = new Point[25]
                {
                point,
                new Point(point.X + 1, point.Y + 1),
                new Point(point.X, point.Y + 1),
                new Point(point.X - 1, point.Y + 1),
                new Point(point.X + 1, point.Y),
                new Point(point.X - 1, point.Y),
                new Point(point.X + 1, point.Y - 1),
                new Point(point.X, point.Y - 1),
                new Point(point.X - 1, point.Y - 1),
                new Point(point.X + 2, point.Y),
                new Point(point.X + 2, point.Y + 1),
                new Point(point.X + 2, point.Y + 2),
                new Point(point.X + 1, point.Y + 2),
                new Point(point.X, point.Y + 2),
                new Point(point.X - 1, point.Y + 2),
                new Point(point.X - 2, point.Y + 2),
                new Point(point.X - 2, point.Y + 1),
                new Point(point.X - 2, point.Y),
                new Point(point.X - 2, point.Y - 1),
                new Point(point.X - 2, point.Y - 2),
                new Point(point.X - 1, point.Y - 2),
                new Point(point.X, point.Y - 2),
                new Point(point.X + 1, point.Y - 2),
                new Point(point.X + 2, point.Y - 2),
                new Point(point.X + 2, point.Y - 1)
                };
                break;
            case ObjectSize.Zhanyue1x3:
                grid = new Point[3]
                {
                point,
                GetNextPosition(point, direction, 1),
                GetNextPosition(point, direction, 2)
                };
                break;
            case ObjectSize.Zhanyue3x3:
                {
                    Point[] array = direction switch
                    {
                        GameDirection.Up => new Point[9]
                        {
                    point,
                    new Point(point.X, point.Y + 1),
                    new Point(point.X, point.Y + 2),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X + 1, point.Y + 2),
                    new Point(point.X - 1, point.Y + 2)
                        },
                        GameDirection.UpLeft => new Point[9]
                        {
                    point,
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X + 2, point.Y + 2),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X + 1, point.Y + 2),
                    new Point(point.X + 2, point.Y + 1)
                        },
                        GameDirection.Left => new Point[9]
                        {
                    point,
                    new Point(point.X + 1, point.Y),
                    new Point(point.X + 2, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X + 2, point.Y - 1),
                    new Point(point.X + 2, point.Y + 1)
                        },
                        GameDirection.Right => new Point[9]
                        {
                    point,
                    new Point(point.X - 1, point.Y),
                    new Point(point.X - 2, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X - 2, point.Y + 1),
                    new Point(point.X - 2, point.Y - 1)
                        },
                        GameDirection.UpRight => new Point[9]
                        {
                    point,
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X - 2, point.Y + 2),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X - 1, point.Y + 2),
                    new Point(point.X - 2, point.Y + 1)
                        },
                        GameDirection.DownLeft => new Point[9]
                        {
                    point,
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X + 2, point.Y - 2),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X + 1, point.Y - 2),
                    new Point(point.X + 2, point.Y - 1)
                        },
                        GameDirection.Down => new Point[9]
                        {
                    point,
                    new Point(point.X, point.Y - 1),
                    new Point(point.X, point.Y - 2),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X - 1, point.Y - 2),
                    new Point(point.X + 1, point.Y - 2)
                        },
                        _ => new Point[9]
                        {
                    point,
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X - 2, point.Y - 2),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 2, point.Y - 1),
                    new Point(point.X - 1, point.Y - 2)
                        },
                    };
                    grid = array;
                    break;
                }
            case ObjectSize.LineType1x5:
                grid = new Point[5]
                {
                point,
                GetNextPosition(point, direction, 1),
                GetNextPosition(point, direction, 2),
                GetNextPosition(point, direction, 3),
                GetNextPosition(point, direction, 4)
                };
                break;
            case ObjectSize.LineType1x8:
                grid = new Point[8]
                {
                point,
                GetNextPosition(point, direction, 1),
                GetNextPosition(point, direction, 2),
                GetNextPosition(point, direction, 3),
                GetNextPosition(point, direction, 4),
                GetNextPosition(point, direction, 5),
                GetNextPosition(point, direction, 6),
                GetNextPosition(point, direction, 7)
                };
                break;
            case ObjectSize.LineType3x8:
                {
                    Point[] array = direction switch
                    {
                        GameDirection.Up => new Point[24]
                        {
                    point,
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X, point.Y + 2),
                    new Point(point.X + 1, point.Y + 2),
                    new Point(point.X - 1, point.Y + 2),
                    new Point(point.X, point.Y + 3),
                    new Point(point.X + 1, point.Y + 3),
                    new Point(point.X - 1, point.Y + 3),
                    new Point(point.X, point.Y + 4),
                    new Point(point.X + 1, point.Y + 4),
                    new Point(point.X - 1, point.Y + 4),
                    new Point(point.X, point.Y + 5),
                    new Point(point.X + 1, point.Y + 5),
                    new Point(point.X - 1, point.Y + 5),
                    new Point(point.X, point.Y + 6),
                    new Point(point.X + 1, point.Y + 6),
                    new Point(point.X - 1, point.Y + 6),
                    new Point(point.X, point.Y + 7),
                    new Point(point.X + 1, point.Y + 7),
                    new Point(point.X - 1, point.Y + 7)
                        },
                        GameDirection.UpLeft => new Point[24]
                        {
                    point,
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 2, point.Y + 2),
                    new Point(point.X + 2, point.Y + 1),
                    new Point(point.X + 1, point.Y + 2),
                    new Point(point.X + 3, point.Y + 3),
                    new Point(point.X + 3, point.Y + 2),
                    new Point(point.X + 2, point.Y + 3),
                    new Point(point.X + 4, point.Y + 4),
                    new Point(point.X + 4, point.Y + 3),
                    new Point(point.X + 3, point.Y + 4),
                    new Point(point.X + 5, point.Y + 5),
                    new Point(point.X + 5, point.Y + 4),
                    new Point(point.X + 4, point.Y + 5),
                    new Point(point.X + 6, point.Y + 6),
                    new Point(point.X + 6, point.Y + 5),
                    new Point(point.X + 5, point.Y + 6),
                    new Point(point.X + 7, point.Y + 7),
                    new Point(point.X + 7, point.Y + 6),
                    new Point(point.X + 6, point.Y + 7)
                        },
                        GameDirection.Left => new Point[24]
                        {
                    point,
                    new Point(point.X, point.Y - 1),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X + 2, point.Y),
                    new Point(point.X + 2, point.Y - 1),
                    new Point(point.X + 2, point.Y + 1),
                    new Point(point.X + 3, point.Y),
                    new Point(point.X + 3, point.Y - 1),
                    new Point(point.X + 3, point.Y + 1),
                    new Point(point.X + 4, point.Y),
                    new Point(point.X + 4, point.Y - 1),
                    new Point(point.X + 4, point.Y + 1),
                    new Point(point.X + 5, point.Y),
                    new Point(point.X + 5, point.Y - 1),
                    new Point(point.X + 5, point.Y + 1),
                    new Point(point.X + 6, point.Y),
                    new Point(point.X + 6, point.Y - 1),
                    new Point(point.X + 6, point.Y + 1),
                    new Point(point.X + 7, point.Y),
                    new Point(point.X + 7, point.Y - 1),
                    new Point(point.X + 7, point.Y + 1)
                        },
                        GameDirection.Right => new Point[24]
                        {
                    point,
                    new Point(point.X, point.Y + 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X - 2, point.Y),
                    new Point(point.X - 2, point.Y + 1),
                    new Point(point.X - 2, point.Y - 1),
                    new Point(point.X - 3, point.Y),
                    new Point(point.X - 3, point.Y + 1),
                    new Point(point.X - 3, point.Y - 1),
                    new Point(point.X - 4, point.Y),
                    new Point(point.X - 4, point.Y + 1),
                    new Point(point.X - 4, point.Y - 1),
                    new Point(point.X - 5, point.Y),
                    new Point(point.X - 5, point.Y + 1),
                    new Point(point.X - 5, point.Y - 1),
                    new Point(point.X - 6, point.Y),
                    new Point(point.X - 6, point.Y + 1),
                    new Point(point.X - 6, point.Y - 1),
                    new Point(point.X - 7, point.Y),
                    new Point(point.X - 7, point.Y + 1),
                    new Point(point.X - 7, point.Y - 1)
                        },
                        GameDirection.UpRight => new Point[24]
                        {
                    point,
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X - 2, point.Y + 2),
                    new Point(point.X - 1, point.Y + 2),
                    new Point(point.X - 2, point.Y + 1),
                    new Point(point.X - 3, point.Y + 3),
                    new Point(point.X - 2, point.Y + 3),
                    new Point(point.X - 3, point.Y + 2),
                    new Point(point.X - 4, point.Y + 4),
                    new Point(point.X - 3, point.Y + 4),
                    new Point(point.X - 4, point.Y + 3),
                    new Point(point.X - 5, point.Y + 5),
                    new Point(point.X - 4, point.Y + 5),
                    new Point(point.X - 5, point.Y + 4),
                    new Point(point.X - 6, point.Y + 6),
                    new Point(point.X - 5, point.Y + 6),
                    new Point(point.X - 6, point.Y + 5),
                    new Point(point.X - 7, point.Y + 7),
                    new Point(point.X - 6, point.Y + 7),
                    new Point(point.X - 7, point.Y + 6)
                        },
                        GameDirection.DownLeft => new Point[24]
                        {
                    point,
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X + 2, point.Y - 2),
                    new Point(point.X + 1, point.Y - 2),
                    new Point(point.X + 2, point.Y - 1),
                    new Point(point.X + 3, point.Y - 3),
                    new Point(point.X + 2, point.Y - 3),
                    new Point(point.X + 3, point.Y - 2),
                    new Point(point.X + 4, point.Y - 4),
                    new Point(point.X + 3, point.Y - 4),
                    new Point(point.X + 4, point.Y - 3),
                    new Point(point.X + 5, point.Y - 5),
                    new Point(point.X + 4, point.Y - 5),
                    new Point(point.X + 5, point.Y - 4),
                    new Point(point.X + 6, point.Y - 6),
                    new Point(point.X + 5, point.Y - 6),
                    new Point(point.X + 6, point.Y - 5),
                    new Point(point.X + 7, point.Y - 7),
                    new Point(point.X + 6, point.Y - 7),
                    new Point(point.X + 7, point.Y - 6)
                        },
                        GameDirection.Down => new Point[24]
                        {
                    point,
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X, point.Y - 2),
                    new Point(point.X - 1, point.Y - 2),
                    new Point(point.X + 1, point.Y - 2),
                    new Point(point.X, point.Y - 3),
                    new Point(point.X - 1, point.Y - 3),
                    new Point(point.X + 1, point.Y - 3),
                    new Point(point.X, point.Y - 4),
                    new Point(point.X - 1, point.Y - 4),
                    new Point(point.X + 1, point.Y - 4),
                    new Point(point.X, point.Y - 5),
                    new Point(point.X - 1, point.Y - 5),
                    new Point(point.X + 1, point.Y - 5),
                    new Point(point.X, point.Y - 6),
                    new Point(point.X - 1, point.Y - 6),
                    new Point(point.X + 1, point.Y - 6),
                    new Point(point.X, point.Y - 7),
                    new Point(point.X - 1, point.Y - 7),
                    new Point(point.X + 1, point.Y - 7)
                        },
                        _ => new Point[24]
                        {
                    point,
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 2, point.Y - 2),
                    new Point(point.X - 2, point.Y - 1),
                    new Point(point.X - 1, point.Y - 2),
                    new Point(point.X - 3, point.Y - 3),
                    new Point(point.X - 3, point.Y - 2),
                    new Point(point.X - 2, point.Y - 3),
                    new Point(point.X - 4, point.Y - 4),
                    new Point(point.X - 4, point.Y - 3),
                    new Point(point.X - 3, point.Y - 4),
                    new Point(point.X - 5, point.Y - 5),
                    new Point(point.X - 5, point.Y - 4),
                    new Point(point.X - 4, point.Y - 5),
                    new Point(point.X - 6, point.Y - 6),
                    new Point(point.X - 6, point.Y - 5),
                    new Point(point.X - 5, point.Y - 6),
                    new Point(point.X - 7, point.Y - 7),
                    new Point(point.X - 7, point.Y - 6),
                    new Point(point.X - 6, point.Y - 7)
                        },
                    };
                    grid = array;
                    break;
                }
            case ObjectSize.Diamond3x3:
                grid = new Point[5]
                {
                point,
                new Point(point.X, point.Y + 1),
                new Point(point.X, point.Y - 1),
                new Point(point.X + 1, point.Y),
                new Point(point.X - 1, point.Y)
                };
                break;
            case ObjectSize.LineType3x7:
                {
                    Point[] array = direction switch
                    {
                        GameDirection.Up => new Point[21]
                        {
                    point,
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X, point.Y + 2),
                    new Point(point.X + 1, point.Y + 2),
                    new Point(point.X - 1, point.Y + 2),
                    new Point(point.X, point.Y + 3),
                    new Point(point.X + 1, point.Y + 3),
                    new Point(point.X - 1, point.Y + 3),
                    new Point(point.X, point.Y + 4),
                    new Point(point.X + 1, point.Y + 4),
                    new Point(point.X - 1, point.Y + 4),
                    new Point(point.X, point.Y + 5),
                    new Point(point.X + 1, point.Y + 5),
                    new Point(point.X - 1, point.Y + 5),
                    new Point(point.X, point.Y + 6),
                    new Point(point.X + 1, point.Y + 6),
                    new Point(point.X - 1, point.Y + 6)
                        },
                        GameDirection.UpLeft => new Point[21]
                        {
                    point,
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 2, point.Y + 2),
                    new Point(point.X + 2, point.Y + 1),
                    new Point(point.X + 1, point.Y + 2),
                    new Point(point.X + 3, point.Y + 3),
                    new Point(point.X + 3, point.Y + 2),
                    new Point(point.X + 2, point.Y + 3),
                    new Point(point.X + 4, point.Y + 4),
                    new Point(point.X + 4, point.Y + 3),
                    new Point(point.X + 3, point.Y + 4),
                    new Point(point.X + 5, point.Y + 5),
                    new Point(point.X + 5, point.Y + 4),
                    new Point(point.X + 4, point.Y + 5),
                    new Point(point.X + 6, point.Y + 6),
                    new Point(point.X + 6, point.Y + 5),
                    new Point(point.X + 5, point.Y + 6)
                        },
                        GameDirection.Left => new Point[21]
                        {
                    point,
                    new Point(point.X, point.Y - 1),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X + 2, point.Y),
                    new Point(point.X + 2, point.Y - 1),
                    new Point(point.X + 2, point.Y + 1),
                    new Point(point.X + 3, point.Y),
                    new Point(point.X + 3, point.Y - 1),
                    new Point(point.X + 3, point.Y + 1),
                    new Point(point.X + 4, point.Y),
                    new Point(point.X + 4, point.Y - 1),
                    new Point(point.X + 4, point.Y + 1),
                    new Point(point.X + 5, point.Y),
                    new Point(point.X + 5, point.Y - 1),
                    new Point(point.X + 5, point.Y + 1),
                    new Point(point.X + 6, point.Y),
                    new Point(point.X + 6, point.Y - 1),
                    new Point(point.X + 6, point.Y + 1)
                        },
                        GameDirection.Right => new Point[21]
                        {
                    point,
                    new Point(point.X, point.Y + 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X - 2, point.Y),
                    new Point(point.X - 2, point.Y + 1),
                    new Point(point.X - 2, point.Y - 1),
                    new Point(point.X - 3, point.Y),
                    new Point(point.X - 3, point.Y + 1),
                    new Point(point.X - 3, point.Y - 1),
                    new Point(point.X - 4, point.Y),
                    new Point(point.X - 4, point.Y + 1),
                    new Point(point.X - 4, point.Y - 1),
                    new Point(point.X - 5, point.Y),
                    new Point(point.X - 5, point.Y + 1),
                    new Point(point.X - 5, point.Y - 1),
                    new Point(point.X - 6, point.Y),
                    new Point(point.X - 6, point.Y + 1),
                    new Point(point.X - 6, point.Y - 1)
                        },
                        GameDirection.UpRight => new Point[21]
                        {
                    point,
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X - 2, point.Y + 2),
                    new Point(point.X - 1, point.Y + 2),
                    new Point(point.X - 2, point.Y + 1),
                    new Point(point.X - 3, point.Y + 3),
                    new Point(point.X - 2, point.Y + 3),
                    new Point(point.X - 3, point.Y + 2),
                    new Point(point.X - 4, point.Y + 4),
                    new Point(point.X - 3, point.Y + 4),
                    new Point(point.X - 4, point.Y + 3),
                    new Point(point.X - 5, point.Y + 5),
                    new Point(point.X - 4, point.Y + 5),
                    new Point(point.X - 5, point.Y + 4),
                    new Point(point.X - 6, point.Y + 6),
                    new Point(point.X - 5, point.Y + 6),
                    new Point(point.X - 6, point.Y + 5)
                        },
                        GameDirection.DownLeft => new Point[21]
                        {
                    point,
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X + 2, point.Y - 2),
                    new Point(point.X + 1, point.Y - 2),
                    new Point(point.X + 2, point.Y - 1),
                    new Point(point.X + 3, point.Y - 3),
                    new Point(point.X + 2, point.Y - 3),
                    new Point(point.X + 3, point.Y - 2),
                    new Point(point.X + 4, point.Y - 4),
                    new Point(point.X + 3, point.Y - 4),
                    new Point(point.X + 4, point.Y - 3),
                    new Point(point.X + 5, point.Y - 5),
                    new Point(point.X + 4, point.Y - 5),
                    new Point(point.X + 5, point.Y - 4),
                    new Point(point.X + 6, point.Y - 6),
                    new Point(point.X + 5, point.Y - 6),
                    new Point(point.X + 6, point.Y - 5)
                        },
                        GameDirection.Down => new Point[21]
                        {
                    point,
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X, point.Y - 2),
                    new Point(point.X - 1, point.Y - 2),
                    new Point(point.X + 1, point.Y - 2),
                    new Point(point.X, point.Y - 3),
                    new Point(point.X - 1, point.Y - 3),
                    new Point(point.X + 1, point.Y - 3),
                    new Point(point.X, point.Y - 4),
                    new Point(point.X - 1, point.Y - 4),
                    new Point(point.X + 1, point.Y - 4),
                    new Point(point.X, point.Y - 5),
                    new Point(point.X - 1, point.Y - 5),
                    new Point(point.X + 1, point.Y - 5),
                    new Point(point.X, point.Y - 6),
                    new Point(point.X - 1, point.Y - 6),
                    new Point(point.X + 1, point.Y - 6)
                        },
                        _ => new Point[21]
                        {
                    point,
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 2, point.Y - 2),
                    new Point(point.X - 2, point.Y - 1),
                    new Point(point.X - 1, point.Y - 2),
                    new Point(point.X - 3, point.Y - 3),
                    new Point(point.X - 3, point.Y - 2),
                    new Point(point.X - 2, point.Y - 3),
                    new Point(point.X - 4, point.Y - 4),
                    new Point(point.X - 4, point.Y - 3),
                    new Point(point.X - 3, point.Y - 4),
                    new Point(point.X - 5, point.Y - 5),
                    new Point(point.X - 5, point.Y - 4),
                    new Point(point.X - 4, point.Y - 5),
                    new Point(point.X - 6, point.Y - 6),
                    new Point(point.X - 6, point.Y - 5),
                    new Point(point.X - 5, point.Y - 6)
                        },
                    };
                    grid = array;
                    break;
                }
            case ObjectSize.Fork3x3:
                grid = new Point[5]
                {
                point,
                new Point(point.X + 1, point.Y + 1),
                new Point(point.X - 1, point.Y + 1),
                new Point(point.X + 1, point.Y - 1),
                new Point(point.X - 1, point.Y - 1)
                };
                break;
            case ObjectSize.Hollow5x5:
                grid = new Point[24]
                {
                new Point(point.X + 1, point.Y + 1),
                new Point(point.X, point.Y + 1),
                new Point(point.X - 1, point.Y + 1),
                new Point(point.X + 1, point.Y),
                new Point(point.X - 1, point.Y),
                new Point(point.X + 1, point.Y - 1),
                new Point(point.X, point.Y - 1),
                new Point(point.X - 1, point.Y - 1),
                new Point(point.X + 2, point.Y),
                new Point(point.X + 2, point.Y + 1),
                new Point(point.X + 2, point.Y + 2),
                new Point(point.X + 1, point.Y + 2),
                new Point(point.X, point.Y + 2),
                new Point(point.X - 1, point.Y + 2),
                new Point(point.X - 2, point.Y + 2),
                new Point(point.X - 2, point.Y + 1),
                new Point(point.X - 2, point.Y),
                new Point(point.X - 2, point.Y - 1),
                new Point(point.X - 2, point.Y - 2),
                new Point(point.X - 1, point.Y - 2),
                new Point(point.X, point.Y - 2),
                new Point(point.X + 1, point.Y - 2),
                new Point(point.X + 2, point.Y - 2),
                new Point(point.X + 2, point.Y - 1)
                };
                break;
            case ObjectSize.LineType1x2:
                grid = new Point[2]
                {
                point,
                GetNextPosition(point, direction, 1)
                };
                break;
            case ObjectSize.Front3x1:
                {
                    Point[] array = direction switch
                    {
                        GameDirection.Up => new Point[3]
                        {
                    point,
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y)
                        },
                        GameDirection.UpLeft => new Point[3]
                        {
                    point,
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y)
                        },
                        GameDirection.Left => new Point[3]
                        {
                    point,
                    new Point(point.X, point.Y - 1),
                    new Point(point.X, point.Y + 1)
                        },
                        GameDirection.Right => new Point[3]
                        {
                    point,
                    new Point(point.X, point.Y + 1),
                    new Point(point.X, point.Y - 1)
                        },
                        GameDirection.UpRight => new Point[3]
                        {
                    point,
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y - 1)
                        },
                        GameDirection.DownLeft => new Point[3]
                        {
                    point,
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y + 1)
                        },
                        GameDirection.Down => new Point[3]
                        {
                    point,
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y)
                        },
                        _ => new Point[3]
                        {
                    point,
                    new Point(point.X, point.Y + 1),
                    new Point(point.X + 1, point.Y)
                        },
                    };
                    grid = array;
                    break;
                }
            case ObjectSize.Spiral7x7:
                grid = new Point[49]
                {
                point,
                new Point(point.X - 1, point.Y),
                new Point(point.X - 1, point.Y - 1),
                new Point(point.X, point.Y - 1),
                new Point(point.X + 1, point.Y - 1),
                new Point(point.X + 1, point.Y),
                new Point(point.X + 1, point.Y + 1),
                new Point(point.X, point.Y + 1),
                new Point(point.X - 1, point.Y + 1),
                new Point(point.X - 2, point.Y + 1),
                new Point(point.X - 2, point.Y),
                new Point(point.X - 2, point.Y - 1),
                new Point(point.X - 2, point.Y - 2),
                new Point(point.X - 1, point.Y - 2),
                new Point(point.X, point.Y - 2),
                new Point(point.X + 1, point.Y - 2),
                new Point(point.X + 2, point.Y - 2),
                new Point(point.X + 2, point.Y - 1),
                new Point(point.X + 2, point.Y),
                new Point(point.X + 2, point.Y + 1),
                new Point(point.X + 2, point.Y + 2),
                new Point(point.X + 1, point.Y + 2),
                new Point(point.X, point.Y + 2),
                new Point(point.X - 1, point.Y + 2),
                new Point(point.X - 2, point.Y + 2),
                new Point(point.X - 3, point.Y + 2),
                new Point(point.X - 3, point.Y + 1),
                new Point(point.X - 3, point.Y),
                new Point(point.X - 3, point.Y - 1),
                new Point(point.X - 3, point.Y - 2),
                new Point(point.X - 3, point.Y - 3),
                new Point(point.X - 2, point.Y - 3),
                new Point(point.X - 1, point.Y - 3),
                new Point(point.X, point.Y - 3),
                new Point(point.X + 1, point.Y - 3),
                new Point(point.X + 2, point.Y - 3),
                new Point(point.X + 3, point.Y - 3),
                new Point(point.X + 3, point.Y - 2),
                new Point(point.X + 3, point.Y - 1),
                new Point(point.X + 3, point.Y),
                new Point(point.X + 3, point.Y + 1),
                new Point(point.X + 3, point.Y + 2),
                new Point(point.X + 3, point.Y + 3),
                new Point(point.X + 2, point.Y + 3),
                new Point(point.X + 1, point.Y + 3),
                new Point(point.X, point.Y + 3),
                new Point(point.X - 1, point.Y + 3),
                new Point(point.X - 2, point.Y + 3),
                new Point(point.X - 3, point.Y + 3)
                };
                break;
            case ObjectSize.Yanlong1x2:
                {
                    Point[] array = direction switch
                    {
                        GameDirection.Up => new Point[6]
                        {
                    point,
                    new Point(point.X, point.Y + 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X + 1, point.Y + 1)
                        },
                        GameDirection.UpLeft => new Point[4]
                        {
                    point,
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y + 1)
                        },
                        GameDirection.Left => new Point[6]
                        {
                    point,
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X + 1, point.Y + 1),
                    new Point(point.X + 1, point.Y - 1)
                        },
                        GameDirection.Right => new Point[6]
                        {
                    point,
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X - 1, point.Y - 1)
                        },
                        GameDirection.UpRight => new Point[4]
                        {
                    point,
                    new Point(point.X - 1, point.Y + 1),
                    new Point(point.X, point.Y + 1),
                    new Point(point.X - 1, point.Y)
                        },
                        GameDirection.DownLeft => new Point[4]
                        {
                    point,
                    new Point(point.X + 1, point.Y - 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X + 1, point.Y)
                        },
                        GameDirection.Down => new Point[6]
                        {
                    point,
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X + 1, point.Y - 1)
                        },
                        _ => new Point[4]
                        {
                    point,
                    new Point(point.X - 1, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X, point.Y - 1)
                        },
                    };
                    grid = array;
                    break;
                }
            case ObjectSize.LineType1x7:
                grid = new Point[7]
                {
                point,
                GetNextPosition(point, direction, 1),
                GetNextPosition(point, direction, 2),
                GetNextPosition(point, direction, 3),
                GetNextPosition(point, direction, 4),
                GetNextPosition(point, direction, 5),
                GetNextPosition(point, direction, 6)
                };
                break;
            case ObjectSize.Spiral15x15:
                grid = new Point[288]
                {
                new Point(point.X - 1, point.Y),
                new Point(point.X - 1, point.Y - 1),
                new Point(point.X, point.Y - 1),
                new Point(point.X + 1, point.Y - 1),
                new Point(point.X + 1, point.Y),
                new Point(point.X + 1, point.Y + 1),
                new Point(point.X, point.Y + 1),
                new Point(point.X - 1, point.Y + 1),
                new Point(point.X - 2, point.Y + 1),
                new Point(point.X - 2, point.Y),
                new Point(point.X - 2, point.Y - 1),
                new Point(point.X - 2, point.Y - 2),
                new Point(point.X - 1, point.Y - 2),
                new Point(point.X, point.Y - 2),
                new Point(point.X + 1, point.Y - 2),
                new Point(point.X + 2, point.Y - 2),
                new Point(point.X + 2, point.Y - 1),
                new Point(point.X + 2, point.Y),
                new Point(point.X + 2, point.Y + 1),
                new Point(point.X + 2, point.Y + 2),
                new Point(point.X + 1, point.Y + 2),
                new Point(point.X, point.Y + 2),
                new Point(point.X - 1, point.Y + 2),
                new Point(point.X - 2, point.Y + 2),
                new Point(point.X - 3, point.Y + 2),
                new Point(point.X - 3, point.Y + 1),
                new Point(point.X - 3, point.Y),
                new Point(point.X - 3, point.Y - 1),
                new Point(point.X - 3, point.Y - 2),
                new Point(point.X - 3, point.Y - 3),
                new Point(point.X - 2, point.Y - 3),
                new Point(point.X - 1, point.Y - 3),
                new Point(point.X, point.Y - 3),
                new Point(point.X + 1, point.Y - 3),
                new Point(point.X + 2, point.Y - 3),
                new Point(point.X + 3, point.Y - 3),
                new Point(point.X + 3, point.Y - 2),
                new Point(point.X + 3, point.Y - 1),
                new Point(point.X + 3, point.Y),
                new Point(point.X + 3, point.Y + 1),
                new Point(point.X + 3, point.Y + 2),
                new Point(point.X + 3, point.Y + 3),
                new Point(point.X + 2, point.Y + 3),
                new Point(point.X + 1, point.Y + 3),
                new Point(point.X, point.Y + 3),
                new Point(point.X - 1, point.Y + 3),
                new Point(point.X - 2, point.Y + 3),
                new Point(point.X - 3, point.Y + 3),
                new Point(point.X - 4, point.Y + 3),
                new Point(point.X - 4, point.Y + 2),
                new Point(point.X - 4, point.Y + 1),
                new Point(point.X - 4, point.Y),
                new Point(point.X - 4, point.Y - 1),
                new Point(point.X - 4, point.Y - 2),
                new Point(point.X - 4, point.Y - 3),
                new Point(point.X - 4, point.Y - 4),
                new Point(point.X - 3, point.Y - 4),
                new Point(point.X - 2, point.Y - 4),
                new Point(point.X - 1, point.Y - 4),
                new Point(point.X, point.Y - 4),
                new Point(point.X + 1, point.Y - 4),
                new Point(point.X + 2, point.Y - 4),
                new Point(point.X + 3, point.Y - 4),
                new Point(point.X + 4, point.Y - 4),
                new Point(point.X + 4, point.Y - 3),
                new Point(point.X + 4, point.Y - 2),
                new Point(point.X + 4, point.Y - 1),
                new Point(point.X + 4, point.Y),
                new Point(point.X + 4, point.Y + 1),
                new Point(point.X + 4, point.Y + 2),
                new Point(point.X + 4, point.Y + 3),
                new Point(point.X + 4, point.Y + 4),
                new Point(point.X + 3, point.Y + 4),
                new Point(point.X + 2, point.Y + 4),
                new Point(point.X + 1, point.Y + 4),
                new Point(point.X, point.Y + 4),
                new Point(point.X - 1, point.Y + 4),
                new Point(point.X - 2, point.Y + 4),
                new Point(point.X - 3, point.Y + 4),
                new Point(point.X - 4, point.Y + 4),
                new Point(point.X - 5, point.Y + 4),
                new Point(point.X - 5, point.Y + 3),
                new Point(point.X - 5, point.Y + 2),
                new Point(point.X - 5, point.Y + 1),
                new Point(point.X - 5, point.Y),
                new Point(point.X - 5, point.Y - 1),
                new Point(point.X - 5, point.Y - 2),
                new Point(point.X - 5, point.Y - 3),
                new Point(point.X - 5, point.Y - 4),
                new Point(point.X - 5, point.Y - 5),
                new Point(point.X - 4, point.Y - 5),
                new Point(point.X - 3, point.Y - 5),
                new Point(point.X - 2, point.Y - 5),
                new Point(point.X - 1, point.Y - 5),
                new Point(point.X, point.Y - 5),
                new Point(point.X + 1, point.Y - 5),
                new Point(point.X + 2, point.Y - 5),
                new Point(point.X + 3, point.Y - 5),
                new Point(point.X + 4, point.Y - 5),
                new Point(point.X + 5, point.Y - 5),
                new Point(point.X + 5, point.Y - 4),
                new Point(point.X + 5, point.Y - 3),
                new Point(point.X + 5, point.Y - 2),
                new Point(point.X + 5, point.Y - 1),
                new Point(point.X + 5, point.Y),
                new Point(point.X + 5, point.Y + 1),
                new Point(point.X + 5, point.Y + 2),
                new Point(point.X + 5, point.Y + 3),
                new Point(point.X + 5, point.Y + 4),
                new Point(point.X + 5, point.Y + 5),
                new Point(point.X + 4, point.Y + 5),
                new Point(point.X + 3, point.Y + 5),
                new Point(point.X + 2, point.Y + 5),
                new Point(point.X + 1, point.Y + 5),
                new Point(point.X, point.Y + 5),
                new Point(point.X - 1, point.Y + 5),
                new Point(point.X - 2, point.Y + 5),
                new Point(point.X - 3, point.Y + 5),
                new Point(point.X - 4, point.Y + 5),
                new Point(point.X - 5, point.Y + 5),
                new Point(point.X - 6, point.Y + 5),
                new Point(point.X - 6, point.Y + 4),
                new Point(point.X - 6, point.Y + 3),
                new Point(point.X - 6, point.Y + 2),
                new Point(point.X - 6, point.Y + 1),
                new Point(point.X - 6, point.Y),
                new Point(point.X - 6, point.Y - 1),
                new Point(point.X - 6, point.Y - 2),
                new Point(point.X - 6, point.Y - 3),
                new Point(point.X - 6, point.Y - 4),
                new Point(point.X - 6, point.Y - 5),
                new Point(point.X - 6, point.Y - 6),
                new Point(point.X - 5, point.Y - 6),
                new Point(point.X - 4, point.Y - 6),
                new Point(point.X - 3, point.Y - 6),
                new Point(point.X - 2, point.Y - 6),
                new Point(point.X - 1, point.Y - 6),
                new Point(point.X, point.Y - 6),
                new Point(point.X + 1, point.Y - 6),
                new Point(point.X + 2, point.Y - 6),
                new Point(point.X + 3, point.Y - 6),
                new Point(point.X + 4, point.Y - 6),
                new Point(point.X + 5, point.Y - 6),
                new Point(point.X + 6, point.Y - 6),
                new Point(point.X + 6, point.Y - 5),
                new Point(point.X + 6, point.Y - 4),
                new Point(point.X + 6, point.Y - 3),
                new Point(point.X + 6, point.Y - 2),
                new Point(point.X + 6, point.Y - 1),
                new Point(point.X + 6, point.Y),
                new Point(point.X + 6, point.Y + 1),
                new Point(point.X + 6, point.Y + 2),
                new Point(point.X + 6, point.Y + 3),
                new Point(point.X + 6, point.Y + 4),
                new Point(point.X + 6, point.Y + 5),
                new Point(point.X + 6, point.Y + 6),
                new Point(point.X + 5, point.Y + 6),
                new Point(point.X + 4, point.Y + 6),
                new Point(point.X + 3, point.Y + 6),
                new Point(point.X + 2, point.Y + 6),
                new Point(point.X + 1, point.Y + 6),
                new Point(point.X, point.Y + 6),
                new Point(point.X - 1, point.Y + 6),
                new Point(point.X - 2, point.Y + 6),
                new Point(point.X - 3, point.Y + 6),
                new Point(point.X - 4, point.Y + 6),
                new Point(point.X - 5, point.Y + 6),
                new Point(point.X - 6, point.Y + 6),
                new Point(point.X - 7, point.Y + 6),
                new Point(point.X - 7, point.Y + 5),
                new Point(point.X - 7, point.Y + 4),
                new Point(point.X - 7, point.Y + 3),
                new Point(point.X - 7, point.Y + 2),
                new Point(point.X - 7, point.Y + 1),
                new Point(point.X - 7, point.Y),
                new Point(point.X - 7, point.Y - 1),
                new Point(point.X - 7, point.Y - 2),
                new Point(point.X - 7, point.Y - 3),
                new Point(point.X - 7, point.Y - 4),
                new Point(point.X - 7, point.Y - 5),
                new Point(point.X - 7, point.Y - 6),
                new Point(point.X - 7, point.Y - 7),
                new Point(point.X - 6, point.Y - 7),
                new Point(point.X - 5, point.Y - 7),
                new Point(point.X - 4, point.Y - 7),
                new Point(point.X - 3, point.Y - 7),
                new Point(point.X - 2, point.Y - 7),
                new Point(point.X - 1, point.Y - 7),
                new Point(point.X, point.Y - 7),
                new Point(point.X + 1, point.Y - 7),
                new Point(point.X + 2, point.Y - 7),
                new Point(point.X + 3, point.Y - 7),
                new Point(point.X + 4, point.Y - 7),
                new Point(point.X + 5, point.Y - 7),
                new Point(point.X + 6, point.Y - 7),
                new Point(point.X + 7, point.Y - 7),
                new Point(point.X + 7, point.Y - 6),
                new Point(point.X + 7, point.Y - 5),
                new Point(point.X + 7, point.Y - 4),
                new Point(point.X + 7, point.Y - 3),
                new Point(point.X + 7, point.Y - 2),
                new Point(point.X + 7, point.Y - 1),
                new Point(point.X + 7, point.Y),
                new Point(point.X + 7, point.Y + 1),
                new Point(point.X + 7, point.Y + 2),
                new Point(point.X + 7, point.Y + 3),
                new Point(point.X + 7, point.Y + 4),
                new Point(point.X + 7, point.Y + 5),
                new Point(point.X + 7, point.Y + 6),
                new Point(point.X + 7, point.Y + 7),
                new Point(point.X + 6, point.Y + 7),
                new Point(point.X + 5, point.Y + 7),
                new Point(point.X + 4, point.Y + 7),
                new Point(point.X + 3, point.Y + 7),
                new Point(point.X + 2, point.Y + 7),
                new Point(point.X + 1, point.Y + 7),
                new Point(point.X, point.Y + 7),
                new Point(point.X - 1, point.Y + 7),
                new Point(point.X - 2, point.Y + 7),
                new Point(point.X - 3, point.Y + 7),
                new Point(point.X - 4, point.Y + 7),
                new Point(point.X - 5, point.Y + 7),
                new Point(point.X - 6, point.Y + 7),
                new Point(point.X - 7, point.Y + 7),
                new Point(point.X - 8, point.Y + 7),
                new Point(point.X - 8, point.Y + 6),
                new Point(point.X - 8, point.Y + 5),
                new Point(point.X - 8, point.Y + 4),
                new Point(point.X - 8, point.Y + 3),
                new Point(point.X - 8, point.Y + 2),
                new Point(point.X - 8, point.Y + 1),
                new Point(point.X - 8, point.Y),
                new Point(point.X - 8, point.Y - 1),
                new Point(point.X - 8, point.Y - 2),
                new Point(point.X - 8, point.Y - 3),
                new Point(point.X - 8, point.Y - 4),
                new Point(point.X - 8, point.Y - 5),
                new Point(point.X - 8, point.Y - 6),
                new Point(point.X - 8, point.Y - 7),
                new Point(point.X - 8, point.Y - 8),
                new Point(point.X - 7, point.Y - 8),
                new Point(point.X - 6, point.Y - 8),
                new Point(point.X - 5, point.Y - 8),
                new Point(point.X - 4, point.Y - 8),
                new Point(point.X - 3, point.Y - 8),
                new Point(point.X - 2, point.Y - 8),
                new Point(point.X - 1, point.Y - 8),
                new Point(point.X, point.Y - 8),
                new Point(point.X + 1, point.Y - 8),
                new Point(point.X + 2, point.Y - 8),
                new Point(point.X + 3, point.Y - 8),
                new Point(point.X + 4, point.Y - 8),
                new Point(point.X + 5, point.Y - 8),
                new Point(point.X + 6, point.Y - 8),
                new Point(point.X + 7, point.Y - 8),
                new Point(point.X + 8, point.Y - 8),
                new Point(point.X + 8, point.Y - 7),
                new Point(point.X + 8, point.Y - 6),
                new Point(point.X + 8, point.Y - 5),
                new Point(point.X + 8, point.Y - 4),
                new Point(point.X + 8, point.Y - 3),
                new Point(point.X + 8, point.Y - 2),
                new Point(point.X + 8, point.Y - 1),
                new Point(point.X + 8, point.Y),
                new Point(point.X + 8, point.Y + 1),
                new Point(point.X + 8, point.Y + 2),
                new Point(point.X + 8, point.Y + 3),
                new Point(point.X + 8, point.Y + 4),
                new Point(point.X + 8, point.Y + 5),
                new Point(point.X + 8, point.Y + 6),
                new Point(point.X + 8, point.Y + 7),
                new Point(point.X + 8, point.Y + 8),
                new Point(point.X + 7, point.Y + 8),
                new Point(point.X + 6, point.Y + 8),
                new Point(point.X + 5, point.Y + 8),
                new Point(point.X + 4, point.Y + 8),
                new Point(point.X + 3, point.Y + 8),
                new Point(point.X + 2, point.Y + 8),
                new Point(point.X + 1, point.Y + 8),
                new Point(point.X, point.Y + 8),
                new Point(point.X - 1, point.Y + 8),
                new Point(point.X - 2, point.Y + 8),
                new Point(point.X - 3, point.Y + 8),
                new Point(point.X - 4, point.Y + 8),
                new Point(point.X - 5, point.Y + 8),
                new Point(point.X - 6, point.Y + 8),
                new Point(point.X - 7, point.Y + 8),
                new Point(point.X - 8, point.Y + 8)
                };
                break;
            case ObjectSize.LineType1x6:
                grid = new Point[6]
                {
                point,
                GetNextPosition(point, direction, 1),
                GetNextPosition(point, direction, 2),
                GetNextPosition(point, direction, 3),
                GetNextPosition(point, direction, 4),
                GetNextPosition(point, direction, 5)
                };
                break;
            default:
                grid = new Point[0];
                break;
        }

        return grid;
    }

    static Compute()
    {
        SystemTime = new DateTime(1970, 1, 1, 8, 0, 0);
    }
}
