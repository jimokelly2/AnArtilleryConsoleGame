namespace ArtilleryGame
{
    public class Game
    {
        private const double GRAVITY = 9.81;
        private const double MUZZLE_VELOCITY_BASE = 100;
        private const double GRID_SIZE = 20;
        private const double HIT_TOLERANCE = 5;

        private Random random = new Random();
        private List<Target> targets = new List<Target>();
        private Observer observer1;
        private Observer observer2;
        private Cannon cannon;
        private Dictionary<int, double> chargeVelocityMultiplier;
        private bool gameOver = false;
        private List<(double x, double y)> impactPoints = new List<(double x, double y)>();

        private readonly int[] convenientAngles = { 0, 30, 45, 60, 90, 120, 135, 150, 180, 210, 225, 240, 270, 300, 315, 330 };

        public void Start()
        {
            bool playAgain = true;

            while (playAgain)
            {
                ResetGameState();

                Console.WriteLine(LanguageManager.GetString("game_title"));
                Console.WriteLine(LanguageManager.GetString("game_desc"));
                Console.WriteLine(LanguageManager.GetString("game_desc2"));

                InitializeScene();
                InitializeChargeTable();

                while (!gameOver)
                {
                    ShowStatus();
                    ProcessCommand(Console.ReadLine()?.ToLower().Trim() ?? "");
                }

                Console.WriteLine("\n" + LanguageManager.GetString("play_again_prompt"));
                string input = Console.ReadLine()?.ToLower().Trim() ?? "";
                playAgain = input == "y" || input == "yes" || input == "是";

                if (!playAgain)
                {
                    Console.WriteLine(LanguageManager.GetString("goodbye"));
                }
            }
        }

        private void ResetGameState()
        {
            gameOver = false;
            targets.Clear();
            impactPoints.Clear();
            cannon = new Cannon();
        }

        private void InitializeScene()
        {
            // 从 convenientAngles 中随机选取两个角度，保证相差约120°
            int angleIndex1 = random.Next(convenientAngles.Length);
            int angleIndex2;

            // 找到与 angleIndex1 相差约120°（即索引差 8）
            // convenientAngles 共16个元素，每个间隔15°，索引差8 = 120°差
            int targetOffset = 8; // 15° × 8 = 120°

            // 随机决定是顺时针偏移还是逆时针偏移
            bool clockwise = random.Next(2) == 0;
            if (clockwise)
                angleIndex2 = (angleIndex1 + targetOffset) % convenientAngles.Length;
            else
                angleIndex2 = (angleIndex1 - targetOffset + convenientAngles.Length) % convenientAngles.Length;

            double angle1 = convenientAngles[angleIndex1];
            double angle2 = convenientAngles[angleIndex2];

            observer1 = new Observer(150 + random.NextDouble() * 150, angle1);
            observer2 = new Observer(150 + random.NextDouble() * 150, angle2);

            int targetCount = random.Next(1, 3);
            for (int i = 0; i < targetCount; i++)
            {
                double[] convenientDistances = { 500, 600, 700, 800 };
                double distance = convenientDistances[random.Next(convenientDistances.Length)];
                double bearing = convenientAngles[random.Next(convenientAngles.Length)];
                targets.Add(new Target(distance, bearing));
            }

            cannon = new Cannon();

            Console.WriteLine(LanguageManager.GetString("scene_init"));
            Console.WriteLine(LanguageManager.GetString("target_count", targets.Count));
            Console.WriteLine(LanguageManager.GetString("observer_pos", 1, observer1.Distance, observer1.Bearing));
            Console.WriteLine(LanguageManager.GetString("observer_pos", 2, observer2.Distance, observer2.Bearing));
            Console.WriteLine(LanguageManager.GetString("artillery_pos"));
        }

        private void InitializeChargeTable()
        {
            chargeVelocityMultiplier = new Dictionary<int, double>
            {
                {1, 1.0},
                {2, 1.4},
                {3, 1.8},
                {4, 2.2},
                {5, 2.7},
                {6, 3.3}
            };

            Console.WriteLine(LanguageManager.GetString("range_info"));
            Console.WriteLine(LanguageManager.GetString("base_velocity", MUZZLE_VELOCITY_BASE));
            Console.WriteLine(LanguageManager.GetString("range_table_header"));
            Console.WriteLine(LanguageManager.GetString("range_table_separator"));
            foreach (var kvp in chargeVelocityMultiplier.OrderBy(x => x.Key))
            {
                double velocity = MUZZLE_VELOCITY_BASE * kvp.Value;
                double maxRange = CalculateRange(velocity, 45);
                Console.WriteLine(LanguageManager.GetString("range_table_row", kvp.Key, velocity, maxRange));
            }
            Console.WriteLine();
        }

        private double CalculateRange(double velocity, double elevationDegrees)
        {
            double thetaRad = elevationDegrees * Math.PI / 180;
            return (velocity * velocity * Math.Sin(2 * thetaRad)) / GRAVITY;
        }

        private void ShowStatus()
        {
            Console.WriteLine(LanguageManager.GetString("cannon_status",
                Cannon.FormatBearing(cannon.Bearing), cannon.Elevation, cannon.Charge));
            double currentVelocity = MUZZLE_VELOCITY_BASE * chargeVelocityMultiplier[cannon.Charge];
            double currentRange = CalculateRange(currentVelocity, cannon.Elevation);
            Console.WriteLine(LanguageManager.GetString("current_range", currentRange));
            Console.WriteLine(LanguageManager.GetString("cmd_report"));
            Console.WriteLine(LanguageManager.GetString("cmd_map"));
            Console.WriteLine(LanguageManager.GetString("cmd_range"));
            Console.WriteLine(LanguageManager.GetString("cmd_turn"));
            Console.WriteLine(LanguageManager.GetString("cmd_elevation"));
            Console.WriteLine(LanguageManager.GetString("cmd_charge"));
            Console.WriteLine(LanguageManager.GetString("cmd_fire"));
            Console.WriteLine(LanguageManager.GetString("cmd_language"));
            Console.Write(LanguageManager.GetString("input_prompt"));
        }

        private void ProcessCommand(string command)
        {
            string[] parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0) return;

            switch (parts[0])
            {
                case "report":
                    ReportTargets();
                    break;

                case "map":
                    if (parts.Length > 1 && double.TryParse(parts[1], out double radius) && radius > 0)
                        GenerateAdvancedMap(radius);
                    else
                        GenerateAdvancedMap();
                    break;

                case "range":
                    ShowRangeTable();
                    break;

                case "turn":
                    if (parts.Length > 1 && double.TryParse(parts[1], out double turnAngle))
                        cannon.Turn(turnAngle);
                    else
                        Console.WriteLine(LanguageManager.GetString("turn_prompt"));
                    break;

                case "elevation":
                    if (parts.Length > 1 && double.TryParse(parts[1], out double elev))
                        cannon.SetElevation(elev);
                    else
                        Console.WriteLine(LanguageManager.GetString("elevation_prompt"));
                    break;

                case "charge":
                    if (parts.Length > 1 && int.TryParse(parts[1], out int chg) && chg >= 1 && chg <= 6)
                        cannon.SetCharge(chg);
                    else
                        Console.WriteLine(LanguageManager.GetString("charge_prompt"));
                    break;

                case "fire":
                    Fire();
                    break;

                case "language":
                    if (parts.Length > 1)
                    {
                        if (LanguageManager.SetLanguage(parts[1]))
                            Console.WriteLine(LanguageManager.GetString("lang_changed"));
                        else
                            Console.WriteLine(LanguageManager.GetString("lang_prompt"));
                    }
                    else
                    {
                        Console.WriteLine(LanguageManager.GetString("lang_prompt"));
                    }
                    break;

                default:
                    Console.WriteLine(LanguageManager.GetString("unknown_cmd"));
                    break;
            }
        }

        private void ReportTargets()
        {
            var aliveTargets = targets.Where(t => !t.IsDestroyed).ToList();
            if (aliveTargets.Count == 0)
            {
                Console.WriteLine(LanguageManager.GetString("all_destroyed"));
                return;
            }

            Console.WriteLine(LanguageManager.GetString("observer_report", aliveTargets.Count));

            int target_index = 0;
            foreach (var target in aliveTargets)
            {
                double angleFromO1 = CalculateBearingFromObserver(observer1, target);
                double angleFromO2 = CalculateBearingFromObserver(observer2, target);

                Console.WriteLine(LanguageManager.GetString("target_bearing", 1, angleFromO1, target_index + 1));
                Console.WriteLine(LanguageManager.GetString("target_bearing", 2, angleFromO2, target_index + 1));
                target_index++;
            }
        }

        private double CalculateBearingFromObserver(Observer obs, Target target)
        {
            double obsAngleMath = (90 - obs.Bearing) * Math.PI / 180;
            double targetAngleMath = (90 - target.Bearing) * Math.PI / 180;

            double ox = obs.Distance * Math.Cos(obsAngleMath);
            double oy = obs.Distance * Math.Sin(obsAngleMath);
            double tx = target.Distance * Math.Cos(targetAngleMath);
            double ty = target.Distance * Math.Sin(targetAngleMath);

            double dx = tx - ox;
            double dy = ty - oy;

            double mathAngle = Math.Atan2(dy, dx) * 180 / Math.PI;
            if (mathAngle < 0) mathAngle += 360;

            double bearing = (90 - mathAngle + 360) % 360;
            return bearing;
        }

        private void ShowRangeTable()
        {
            Console.WriteLine(LanguageManager.GetString("range_table_title"));
            Console.WriteLine(LanguageManager.GetString("range_table_header"));
            Console.WriteLine(LanguageManager.GetString("range_table_separator"));
            foreach (var kvp in chargeVelocityMultiplier.OrderBy(x => x.Key))
            {
                double velocity = MUZZLE_VELOCITY_BASE * kvp.Value;
                double maxRange = CalculateRange(velocity, 45);
                Console.WriteLine(LanguageManager.GetString("range_table_row", kvp.Key, velocity, maxRange));
            }
        }

        private void Fire()
        {
            if (!chargeVelocityMultiplier.ContainsKey(cannon.Charge))
            {
                Console.WriteLine(LanguageManager.GetString("invalid_charge"));
                return;
            }

            double velocity = MUZZLE_VELOCITY_BASE * chargeVelocityMultiplier[cannon.Charge];
            double thetaRad = cannon.Elevation * Math.PI / 180;
            double range = (velocity * velocity * Math.Sin(2 * thetaRad)) / GRAVITY;

            double bearingMath = (90 - cannon.Bearing) * Math.PI / 180;
            double impactX = range * Math.Cos(bearingMath);
            double impactY = range * Math.Sin(bearingMath);

            impactPoints.Add((impactX, impactY));

            Console.WriteLine(LanguageManager.GetString("fire_report"));
            Console.WriteLine(LanguageManager.GetString("fire_velocity", velocity, cannon.Elevation));
            Console.WriteLine(LanguageManager.GetString("fire_range", range));
            Console.WriteLine(LanguageManager.GetString("fire_impact", impactX, impactY));

            var aliveTargets = targets.Where(t => !t.IsDestroyed).ToList();
            bool hitAny = false;

            foreach (var target in aliveTargets)
            {
                double targetBearingMath = (90 - target.Bearing) * Math.PI / 180;
                double targetX = target.Distance * Math.Cos(targetBearingMath);
                double targetY = target.Distance * Math.Sin(targetBearingMath);

                double distance = Math.Sqrt(Math.Pow(impactX - targetX, 2) + Math.Pow(impactY - targetY, 2));

                if (distance <= HIT_TOLERANCE)
                {
                    target.IsDestroyed = true;
                    hitAny = true;
                    Console.WriteLine(LanguageManager.GetString("hit", distance));
                }
                else
                {
                    double targetAngleMath = Math.Atan2(targetY, targetX) * 180 / Math.PI;
                    if (targetAngleMath < 0) targetAngleMath += 360;
                    double targetBearing = (90 - targetAngleMath + 360) % 360;

                    double impactAngleMath = Math.Atan2(impactY, impactX) * 180 / Math.PI;
                    if (impactAngleMath < 0) impactAngleMath += 360;
                    double impactBearing = (90 - impactAngleMath + 360) % 360;

                    double angleDiff = Math.Abs(targetBearing - impactBearing);
                    if (angleDiff > 180) angleDiff = 360 - angleDiff;

                    double distanceDiff = Math.Abs(range - target.Distance);

                    Console.WriteLine(LanguageManager.GetString("miss"));
                    Console.WriteLine(LanguageManager.GetString("bearing_error", angleDiff));
                    Console.WriteLine(LanguageManager.GetString("distance_error", distanceDiff));
                    Console.WriteLine(LanguageManager.GetString("total_distance", distance));
                }
            }

            if (hitAny)
            {
                CheckVictory();
            }
        }

        private void CheckVictory()
        {
            if (targets.All(t => t.IsDestroyed))
            {
                Console.WriteLine(LanguageManager.GetString("victory"));
                gameOver = true;
            }
        }

        private void GenerateAdvancedMap(double? focusRadius = null)
        {
            var aliveTargets = targets.Where(t => !t.IsDestroyed).ToList();
            if (aliveTargets.Count == 0 && impactPoints.Count == 0)
            {
                Console.WriteLine(LanguageManager.GetString("excel_no_targets"));
                return;
            }

            // 计算所有实体坐标
            double o1AngleMath = (90 - observer1.Bearing) * Math.PI / 180;
            double o1x = observer1.Distance * Math.Cos(o1AngleMath);
            double o1y = observer1.Distance * Math.Sin(o1AngleMath);

            double o2AngleMath = (90 - observer2.Bearing) * Math.PI / 180;
            double o2x = observer2.Distance * Math.Cos(o2AngleMath);
            double o2y = observer2.Distance * Math.Sin(o2AngleMath);

            // 计算目标坐标
            List<(double x, double y, Target target)> targetCoords = new List<(double, double, Target)>();
            foreach (var target in aliveTargets)
            {
                double targetAngleMath = (90 - target.Bearing) * Math.PI / 180;
                double tx = target.Distance * Math.Cos(targetAngleMath);
                double ty = target.Distance * Math.Sin(targetAngleMath);
                targetCoords.Add((tx, ty, target));
            }

            double minX, maxX, minY, maxY;

            if (focusRadius.HasValue && focusRadius.Value > 0)
            {
                double centerX = 0, centerY = 0;
                double totalWeight = 3;
                centerX = (0 * 3 + o1x * 1 + o2x * 1) / totalWeight;
                centerY = (0 * 3 + o1y * 1 + o2y * 1) / totalWeight;
                minX = centerX - focusRadius.Value;
                maxX = centerX + focusRadius.Value;
                minY = centerY - focusRadius.Value;
                maxY = centerY + focusRadius.Value;
            }
            else
            {
                minX = 0; maxX = 0; minY = 0; maxY = 0;
                UpdateBounds(ref minX, ref maxX, ref minY, ref maxY, 0, 0);
                UpdateBounds(ref minX, ref maxX, ref minY, ref maxY, o1x, o1y);
                UpdateBounds(ref minX, ref maxX, ref minY, ref maxY, o2x, o2y);
                foreach (var tc in targetCoords)
                    UpdateBounds(ref minX, ref maxX, ref minY, ref maxY, tc.x, tc.y);
                foreach (var impact in impactPoints)
                    UpdateBounds(ref minX, ref maxX, ref minY, ref maxY, impact.x, impact.y);
                double rangeX = maxX - minX;
                double rangeY = maxY - minY;
                double margin = Math.Max(rangeX, rangeY) * 0.15;
                minX -= margin;
                maxX += margin;
                minY -= margin;
                maxY += margin;
            }

            double viewRangeX = maxX - minX;
            double viewRangeY = maxY - minY;

            int gridCols = 51;
            int gridRows = (int)(gridCols * (viewRangeY / viewRangeX));
            if (gridRows < 21) gridRows = 21;
            if (gridRows > 41) gridRows = 41;

            char[,] map = new char[gridRows, gridCols];
            for (int r = 0; r < gridRows; r++)
                for (int c = 0; c < gridCols; c++)
                    map[r, c] = '.';

            (int row, int col) ToGrid(double x, double y)
            {
                int col = (int)((x - minX) / viewRangeX * (gridCols - 1));
                int row = (int)((maxY - y) / viewRangeY * (gridRows - 1));
                return (Math.Max(0, Math.Min(gridRows - 1, row)), Math.Max(0, Math.Min(gridCols - 1, col)));
            }

            // 标记炮兵阵地
            var pPos = ToGrid(0, 0);
            map[pPos.row, pPos.col] = 'P';

            // 标记观察员
            var o1Pos = ToGrid(o1x, o1y);
            map[o1Pos.row, o1Pos.col] = '1';
            var o2Pos = ToGrid(o2x, o2y);
            map[o2Pos.row, o2Pos.col] = '2';

            // 标记目标
            foreach (var tc in targetCoords)
            {
                var tPos = ToGrid(tc.x, tc.y);
                if (map[tPos.row, tPos.col] == '.')
                    map[tPos.row, tPos.col] = 'T';
            }

            // 标记弹着点
            foreach (var impact in impactPoints)
            {
                var iPos = ToGrid(impact.x, impact.y);
                if (map[iPos.row, iPos.col] == '.' || map[iPos.row, iPos.col] == 'T')
                    map[iPos.row, iPos.col] = 'X';
            }

            // 输出高级地图
            Console.WriteLine();
            Console.WriteLine("========================================" +
                              "==============================");
            Console.WriteLine("  " + LanguageManager.GetString("excel_title"));
            Console.WriteLine("========================================" +
                              "==============================");

            // 信息面板
            Console.WriteLine("  | Artillery: (0, 0)");
            Console.WriteLine("  | Obs 1: (" + o1x.ToString("F0") + ", " + o1y.ToString("F0") + ")  " +
                              "Bearing: " + observer1.Bearing.ToString("F1") + " deg");
            Console.WriteLine("  | Obs 2: (" + o2x.ToString("F0") + ", " + o2y.ToString("F0") + ")  " +
                              "Bearing: " + observer2.Bearing.ToString("F1") + " deg");
            Console.WriteLine("  | Targets alive: " + aliveTargets.Count);
            Console.WriteLine("  | Shots fired: " + impactPoints.Count);
            Console.WriteLine("----------------------------------------" +
                              "------------------------------");

            // 地图区域
            Console.WriteLine();

            // 上边框
            Console.Write("  +");
            for (int c = 0; c < gridCols; c++)
                Console.Write("-");
            Console.WriteLine("+");

            // 地图内容
            for (int r = 0; r < gridRows; r++)
            {
                Console.Write("  |");
                for (int c = 0; c < gridCols; c++)
                {
                    char ch = map[r, c];
                    switch (ch)
                    {
                        case 'P':
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case '1':
                        case '2':
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case 'T':
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case 'X':
                            // 检查是否是命中点
                            bool isHit = false;
                            foreach (var impact in impactPoints)
                            {
                                var iPos = ToGrid(impact.x, impact.y);
                                if (iPos.row == r && iPos.col == c)
                                {
                                    foreach (var target in targets.Where(t => t.IsDestroyed))
                                    {
                                        double targetAngleMath = (90 - target.Bearing) * Math.PI / 180;
                                        double tx = target.Distance * Math.Cos(targetAngleMath);
                                        double ty = target.Distance * Math.Sin(targetAngleMath);
                                        double dist = Math.Sqrt(Math.Pow(impact.x - tx, 2) + Math.Pow(impact.y - ty, 2));
                                        if (dist <= HIT_TOLERANCE)
                                        {
                                            isHit = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (isHit)
                                Console.ForegroundColor = ConsoleColor.Magenta;
                            else
                                Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                    }
                    Console.Write(ch);
                    Console.ResetColor();
                }
                Console.WriteLine("|");
            }

            // 下边框
            Console.Write("  +");
            for (int c = 0; c < gridCols; c++)
                Console.Write("-");
            Console.WriteLine("+");

            // 图例
            Console.WriteLine();
            Console.Write("  Legend: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("P");
            Console.ResetColor();
            Console.Write("=Artillery ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("1/2");
            Console.ResetColor();
            Console.Write("=Observer ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("T");
            Console.ResetColor();
            Console.Write("=Target ");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("X");
            Console.ResetColor();
            Console.Write("=Miss ");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("X");
            Console.ResetColor();
            Console.WriteLine("=Hit");

            // 偏差分析面板
            if (impactPoints.Count > 0)
            {
                Console.WriteLine("----------------------------------------" +
                                  "------------------------------");
                Console.WriteLine("  Deviation Analysis:");

                var lastImpact = impactPoints.Last();
                foreach (var tc in targetCoords)
                {
                    double dist = Math.Sqrt(Math.Pow(lastImpact.x - tc.x, 2) + Math.Pow(lastImpact.y - tc.y, 2));

                    Console.Write("    Distance: " + dist.ToString("F0") + "m  ");

                    if (dist <= HIT_TOLERANCE)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[DIRECT HIT]");
                    }
                    else if (dist <= HIT_TOLERANCE * 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("[NEAR MISS]");
                    }
                    else if (dist <= HIT_TOLERANCE * 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[OFF TARGET]");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("[WAY OFF]");
                    }
                    Console.ResetColor();
                }
            }

            // 网格信息
            Console.WriteLine("----------------------------------------" +
                              "------------------------------");
            Console.WriteLine("  Grid: X[" + minX.ToString("F0") + ", " + maxX.ToString("F0") + "]  " +
                              "Y[" + minY.ToString("F0") + ", " + maxY.ToString("F0") + "]");
            Console.WriteLine("  Resolution: " + (viewRangeX / gridCols).ToString("F0") + "x" +
                              (viewRangeY / gridRows).ToString("F0") + " m/cell");
            Console.WriteLine("========================================" +
                              "==============================");
        }

        private void UpdateBounds(ref double minX, ref double maxX, ref double minY, ref double maxY, double x, double y)
        {
            minX = Math.Min(minX, x);
            maxX = Math.Max(maxX, x);
            minY = Math.Min(minY, y);
            maxY = Math.Max(maxY, y);
        }
    }
}