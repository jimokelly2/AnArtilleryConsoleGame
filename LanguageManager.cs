namespace ArtilleryGame
{
    public class LanguageManager
    {
        private static Dictionary<string, Dictionary<string, string>> translations;
        public static string CurrentLanguage { get; private set; } = "en";

        static LanguageManager()
        {
            translations = new Dictionary<string, Dictionary<string, string>>();

            // 中文
            translations["zh-CN"] = new Dictionary<string, string>
            {
                {"game_title", "=== 火炮射击模拟游戏 (理想弹道模型) ==="},
                {"game_desc", "您指挥一个炮兵阵地，需要摧毁所有敌方目标。"},
                {"game_desc2", "使用理想弹道模型：忽略空气阻力、炮弹自重、地球自转"},
                {"scene_init", "\n场景已初始化！"},
                {"target_count", "目标数量: {0} (位置未知)"},
                {"observer_pos", "观察员{0}位置: 距离原点{1:F0}米, 方位角{2:F1}°"},
                {"artillery_pos", "炮兵阵地位于原点 (0,0)\n"},
                {"cannon_status", "\n[火炮状态] 方向角: {0:F1}°, 仰角: {1:F1}°, 装药量: {2}"},
                {"current_range", "当前理论射程: {0:F0}米"},
                {"cmd_report", "  report         - 请求观察员报告目标方位"},
                {"cmd_map", "  map          - 在中生成坐标示意图"},
                {"cmd_range", "  range          - 显示射程表"},
                {"cmd_turn", "  turn [角度]     - 转动火炮方向 (例: turn 45)"},
                {"cmd_elevation", "  elevation [角度] - 调整仰角 (例: elevation 40)"},
                {"cmd_charge", "  charge [1-6]    - 设置装药量 (例: charge 3)"},
                {"cmd_fire", "  fire           - 开火!"},
                {"cmd_language", "  language [en/de/zh] - 切换语言"},
                {"input_prompt", "请输入命令: "},
                {"all_destroyed", "所有目标已被摧毁！"},
                {"observer_report", "\n=== 观察员报告 (剩余{0}个目标) ==="},
                {"target_bearing", "目标{2}方位 - 观察员{0}视角: {1:F1}°"},
                {"range_table_title", "\n=== 理想弹道射程表 ==="},
                {"range_table_header", "装药量 | 初速度(m/s) | 最大射程(米) @45°"},
                {"range_table_separator", "----------------------------------------"},
                {"range_table_row", "   {0}    |   {1:F0}      |   {2:F0}"},
                {"invalid_charge", "无效的装药量！"},
                {"fire_report", "\n=== 炮击报告 (理想弹道) ==="},
                {"fire_velocity", "初速度: {0:F0} m/s, 仰角: {1:F1}°"},
                {"fire_range", "理论射程: {0:F0}米"},
                {"fire_impact", "弹着点: ({0:F1}, {1:F1})"},
                {"hit", "✅ 目标被命中并摧毁！(偏差{0:F1}米)"},
                {"miss", "❌ 未命中目标。"},
                {"bearing_error", "   方位角偏差: {0:F1}°"},
                {"distance_error", "   距离偏差: {0:F0}米"},
                {"total_distance", "   与目标直线距离: {0:F0}米"},
                {"victory", "\n🎉 恭喜！所有目标已被摧毁！游戏胜利！"},
                {"unknown_cmd", "未知命令，请重新输入"},
                {"turn_prompt", "请指定转向角度，例如: turn 45"},
                {"elevation_prompt", "请指定仰角，例如: elevation 35"},
                {"charge_prompt", "装药量范围为1-6，例如: charge 3"},
                {"cannon_turned", "火炮转向至 {0:F1}°"},
                {"cannon_elevation", "火炮仰角调整为 {0:F1}°"},
                {"cannon_charged", "装药量设置为 {0}"},
                {"lang_changed", "语言已切换为中文"},
                {"lang_prompt", "支持的语言: en (英语), de (德语), zh (中文)"},
                {"range_info", "基于公式: R = v₀²·sin(2θ)/g"},
                {"base_velocity", "基础初速度: {0} m/s, 最佳仰角: 45°\n"},
                {"excel_generating", "正在生成坐标示意图..."},
                {"excel_no_targets", "没有剩余目标可显示"},
                {"excel_title", "火炮射击坐标示意图"},
                {"excel_legend", "图例: P=炮兵阵地 O=观察员 T=目标(隐藏) X=弹着点"},
                {"play_again_prompt", "是否再来一局？(y/n): "},
                {"goodbye", "感谢游玩，再见！"},
                {"dir_north", "正北"},
                {"dir_northeast", "东北"},
                {"dir_east", "正东"},
                {"dir_southeast", "东南"},
                {"dir_south", "正南"},
                {"dir_southwest", "西南"},
                {"dir_west", "正西"},
                {"dir_northwest", "西北"}
            };

            // 英文
            translations["en"] = new Dictionary<string, string>
            {
                {"game_title", "=== Artillery Simulation Game (Ideal Trajectory) ==="},
                {"game_desc", "You command an artillery position and need to destroy all enemy targets."},
                {"game_desc2", "Using ideal trajectory model: ignoring air resistance, shell weight, Earth's rotation"},
                {"scene_init", "\nScene initialized!"},
                {"target_count", "Target count: {0} (positions unknown)"},
                {"observer_pos", "Observer {0}: Distance {1:F0}m, Bearing {2:F1}°"},
                {"artillery_pos", "Artillery position at origin (0,0)\n"},
                {"cannon_status", "\n[Cannon Status] Bearing: {0:F1}°, Elevation: {1:F1}°, Charge: {2}"},
                {"current_range", "Current theoretical range: {0:F0}m"},
                {"cmd_report", "  report         - Request observers to report target bearings"},
                {"cmd_map", "  map          - Generate coordinate map"},
                {"cmd_range", "  range          - Display range table"},
                {"cmd_turn", "  turn [angle]   - Rotate cannon (e.g., turn 45)"},
                {"cmd_elevation", "  elevation [angle] - Adjust elevation (e.g., elevation 40)"},
                {"cmd_charge", "  charge [1-6]   - Set charge level (e.g., charge 3)"},
                {"cmd_fire", "  fire           - Fire!"},
                {"cmd_language", "  language [en/de/zh] - Switch language"},
                {"input_prompt", "Enter command: "},
                {"all_destroyed", "All targets have been destroyed!"},
                {"observer_report", "\n=== Observer Report ({0} targets remaining) ==="},
                {"target_bearing", "Target {2} bearing - Observer {0}: {1:F1}°"},
                {"range_table_title", "\n=== Ideal Trajectory Range Table ==="},
                {"range_table_header", "Charge | Velocity(m/s) | Max Range(m) @45°"},
                {"range_table_separator", "----------------------------------------"},
                {"range_table_row", "   {0}    |   {1:F0}      |   {2:F0}"},
                {"invalid_charge", "Invalid charge level!"},
                {"fire_report", "\n=== Firing Report (Ideal Trajectory) ==="},
                {"fire_velocity", "Muzzle velocity: {0:F0} m/s, Elevation: {1:F1}°"},
                {"fire_range", "Theoretical range: {0:F0}m"},
                {"fire_impact", "Impact point: ({0:F1}, {1:F1})"},
                {"hit", "✅ Target hit and destroyed! (Deviation: {0:F1}m)"},
                {"miss", "❌ Target missed."},
                {"bearing_error", "   Bearing error: {0:F1}°"},
                {"distance_error", "   Distance error: {0:F0}m"},
                {"total_distance", "   Direct distance to target: {0:F0}m"},
                {"victory", "\n🎉 Congratulations! All targets destroyed! Victory!"},
                {"unknown_cmd", "Unknown command, please try again"},
                {"turn_prompt", "Please specify rotation angle, e.g.: turn 45"},
                {"elevation_prompt", "Please specify elevation angle, e.g.: elevation 35"},
                {"charge_prompt", "Charge level must be 1-6, e.g.: charge 3"},
                {"cannon_turned", "Cannon rotated to {0:F1}°"},
                {"cannon_elevation", "Cannon elevation set to {0:F1}°"},
                {"cannon_charged", "Charge level set to {0}"},
                {"lang_changed", "Language switched to English"},
                {"lang_prompt", "Available languages: en (English), de (German), zh (Chinese)"},
                {"range_info", "Based on formula: R = v₀²·sin(2θ)/g"},
                {"base_velocity", "Base muzzle velocity: {0} m/s, Optimal elevation: 45°\n"},
                {"excel_generating", "Generating coordinate diagram..."},
                {"excel_no_targets", "No remaining targets to display"},
                {"excel_title", "Artillery Coordinate Map"},
                {"excel_legend", "Legend: P=Artillery O=Observer T=Target(hidden) X=Impact"},
                {"play_again_prompt", "Play again? (y/n): "},
                {"goodbye", "Thanks for playing, goodbye!"},
                {"dir_north", "North"},
                {"dir_northeast", "Northeast"},
                {"dir_east", "East"},
                {"dir_southeast", "Southeast"},
                {"dir_south", "South"},
                {"dir_southwest", "Southwest"},
                {"dir_west", "West"},
                {"dir_northwest", "Northwest"}
            };

            // 德语
            translations["de"] = new Dictionary<string, string>
            {
                {"game_title", "=== Artillerie-Simulationsspiel (Ideale Flugbahn) ==="},
                {"game_desc", "Sie kommandieren eine Artilleriestellung und müssen alle feindlichen Ziele zerstören."},
                {"game_desc2", "Verwendung des idealen Flugbahnmodells: ohne Luftwiderstand, Projektilgewicht, Erdrotation"},
                {"scene_init", "\nSzene initialisiert!"},
                {"target_count", "Anzahl der Ziele: {0} (Positionen unbekannt)"},
                {"observer_pos", "Beobachter {0}: Entfernung {1:F0}m, Peilung {2:F1}°"},
                {"artillery_pos", "Artilleriestellung am Ursprung (0,0)\n"},
                {"cannon_status", "\n[Geschützstatus] Richtung: {0:F1}°, Höhe: {1:F1}°, Ladung: {2}"},
                {"current_range", "Aktuelle theoretische Reichweite: {0:F0}m"},
                {"cmd_report", "  report         - Beobachter melden Zielpeilungen"},
                {"cmd_map", "  map          - Koordinatenkarte generieren"},
                {"cmd_range", "  range          - Reichweitentabelle anzeigen"},
                {"cmd_turn", "  turn [Winkel]  - Geschütz drehen (z.B. turn 45)"},
                {"cmd_elevation", "  elevation [Winkel] - Höhe einstellen (z.B. elevation 40)"},
                {"cmd_charge", "  charge [1-6]   - Ladungsstufe setzen (z.B. charge 3)"},
                {"cmd_fire", "  fire           - Feuer!"},
                {"cmd_language", "  language [en/de/zh] - Sprache wechseln"},
                {"input_prompt", "Befehl eingeben: "},
                {"all_destroyed", "Alle Ziele wurden zerstört!"},
                {"observer_report", "\n=== Beobachterbericht ({0} Ziele übrig) ==="},
                {"target_bearing", "Zielpeilung {2} - Beobachter {0}: {1:F1}°"},
                {"range_table_title", "\n=== Ideale Flugbahn-Reichweitentabelle ==="},
                {"range_table_header", "Ladung | Geschwindigkeit(m/s) | Max. Reichweite(m) @45°"},
                {"range_table_separator", "----------------------------------------"},
                {"range_table_row", "   {0}    |   {1:F0}      |   {2:F0}"},
                {"invalid_charge", "Ungültige Ladungsstufe!"},
                {"fire_report", "\n=== Abschussbericht (Ideale Flugbahn) ==="},
                {"fire_velocity", "Mündungsgeschwindigkeit: {0:F0} m/s, Höhe: {1:F1}°"},
                {"fire_range", "Theoretische Reichweite: {0:F0}m"},
                {"fire_impact", "Einschlagpunkt: ({0:F1}, {1:F1})"},
                {"hit", "✅ Ziel getroffen und zerstört! (Abweichung: {0:F1}m)"},
                {"miss", "❌ Ziel verfehlt."},
                {"bearing_error", "   Peilungsabweichung: {0:F1}°"},
                {"distance_error", "   Entfernungsabweichung: {0:F0}m"},
                {"total_distance", "   Direkte Entfernung zum Ziel: {0:F0}m"},
                {"victory", "\n🎉 Glückwunsch! Alle Ziele zerstört! Sieg!"},
                {"unknown_cmd", "Unbekannter Befehl, bitte versuchen Sie es erneut"},
                {"turn_prompt", "Bitte Drehwinkel angeben, z.B.: turn 45"},
                {"elevation_prompt", "Bitte Höhenwinkel angeben, z.B.: elevation 35"},
                {"charge_prompt", "Ladungsstufe muss 1-6 sein, z.B.: charge 3"},
                {"cannon_turned", "Geschütz auf {0:F1}° gedreht"},
                {"cannon_elevation", "Geschützhöhe auf {0:F1}° eingestellt"},
                {"cannon_charged", "Ladungsstufe auf {0} gesetzt"},
                {"lang_changed", "Sprache auf Deutsch umgestellt"},
                {"lang_prompt", "Verfügbare Sprachen: en (Englisch), de (Deutsch), zh (Chinesisch)"},
                {"range_info", "Basierend auf Formel: R = v₀²·sin(2θ)/g"},
                {"base_velocity", "Basis-Mündungsgeschwindigkeit: {0} m/s, Optimale Höhe: 45°\n"},
                {"excel_generating", "Koordinatendiagramm wird generiert..."},
                {"excel_no_targets", "Keine verbleibenden Ziele zum Anzeigen"},
                {"excel_title", "Artillerie-Koordinatenkarte"},
                {"excel_legend", "Legende: P=Artillerie O=Beobachter T=Ziel(versteckt) X=Einschlag"},
                {"play_again_prompt", "Noch eine Runde? (y/n): "},
                {"goodbye", "Danke fürs Spielen, auf Wiedersehen!"},
                {"dir_north", "Norden"},
                {"dir_northeast", "Nordosten"},
                {"dir_east", "Osten"},
                {"dir_southeast", "Südosten"},
                {"dir_south", "Süden"},
                {"dir_southwest", "Südwesten"},
                {"dir_west", "Westen"},
                {"dir_northwest", "Nordwesten"}
            };
        }

        public static string GetString(string key, params object[] args)
        {
            if (translations.ContainsKey(CurrentLanguage) &&
                translations[CurrentLanguage].ContainsKey(key))
            {
                string template = translations[CurrentLanguage][key];
                return args.Length > 0 ? string.Format(template, args) : template;
            }
            return $"[{key}]";
        }

        public static bool SetLanguage(string langCode)
        {
            switch (langCode.ToLower())
            {
                case "zh":
                case "zh-cn":
                    CurrentLanguage = "zh-CN";
                    return true;
                case "en":
                    CurrentLanguage = "en";
                    return true;
                case "de":
                    CurrentLanguage = "de";
                    return true;
                default:
                    return false;
            }
        }
    }
}