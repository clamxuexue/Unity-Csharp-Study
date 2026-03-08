using System;

namespace MulticastDelegates
{
    /// <summary>
    /// 03 - 多播委托示例
    /// 多播委托可以引用多个方法，调用时会按顺序执行所有方法
    /// </summary>
    class MulticastDelegateExample
    {
        // ========== 委托声明 ==========

        /// <summary>
        /// 通知委托：用于发送通知消息
        /// </summary>
        public delegate void NotificationDelegate(string message);

        /// <summary>
        /// 日志委托：用于记录日志
        /// </summary>
        public delegate void LogDelegate(string logMessage);

        /// <summary>
        /// 计算委托：用于数学运算（有返回值）
        /// </summary>
        public delegate int CalculationDelegate(int x, int y);

        // ========== 通知方法 ==========

        /// <summary>
        /// 发送邮件通知
        /// </summary>
        public static void SendEmail(string message)
        {
            Console.WriteLine($"📧 [邮件] 发送邮件: {message}");
        }

        /// <summary>
        /// 发送短信通知
        /// </summary>
        public static void SendSMS(string message)
        {
            Console.WriteLine($"📱 [短信] 发送短信: {message}");
        }

        /// <summary>
        /// 发送推送通知
        /// </summary>
        public static void SendPushNotification(string message)
        {
            Console.WriteLine($"🔔 [推送] 发送推送通知: {message}");
        }

        /// <summary>
        /// 记录到控制台
        /// </summary>
        public static void LogToConsole(string message)
        {
            Console.WriteLine($"[控制台] {DateTime.Now:HH:mm:ss} - {message}");
        }

        // ========== 日志方法 ==========

        /// <summary>
        /// 记录到文件（模拟）
        /// </summary>
        public static void LogToFile(string message)
        {
            Console.WriteLine($"[文件] {DateTime.Now:HH:mm:ss} - 写入日志文件: {message}");
        }

        /// <summary>
        /// 记录到数据库（模拟）
        /// </summary>
        public static void LogToDatabase(string message)
        {
            Console.WriteLine($"[数据库] {DateTime.Now:HH:mm:ss} - 写入数据库: {message}");
        }

        // ========== 计算方法 ==========

        /// <summary>
        /// 加法运算
        /// </summary>
        public static int Add(int x, int y)
        {
            int result = x + y;
            Console.WriteLine($"加法: {x} + {y} = {result}");
            return result;
        }

        /// <summary>
        /// 乘法运算
        /// </summary>
        public static int Multiply(int x, int y)
        {
            int result = x * y;
            Console.WriteLine($"乘法: {x} × {y} = {result}");
            return result;
        }

        /// <summary>
        /// 减法运算
        /// </summary>
        public static int Subtract(int x, int y)
        {
            int result = x - y;
            Console.WriteLine($"减法: {x} - {y} = {result}");
            return result;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("========== 多播委托示例 ==========\n");

            // ========== 示例1：创建多播委托 ==========
            Console.WriteLine("【示例1：创建多播委托】");

            // 方法1：使用 += 运算符添加方法
            NotificationDelegate notification = null;
            notification += SendEmail;
            notification += SendSMS;
            notification += SendPushNotification;

            // 调用多播委托，会依次执行所有方法
            Console.WriteLine("调用多播委托:");
            notification("系统更新通知");

            Console.WriteLine();

            // ========== 示例2：移除委托中的方法 ==========
            Console.WriteLine("【示例2：移除委托中的方法】");

            // 使用 -= 运算符移除方法
            notification -= SendSMS;

            Console.WriteLine("移除短信通知后:");
            notification("新消息提醒");

            Console.WriteLine();

            // ========== 示例3：多播委托的另一种创建方式 ==========
            Console.WriteLine("【示例3：使用Delegate.Combine创建多播委托】");

            LogDelegate logger1 = LogToConsole;
            LogDelegate logger2 = LogToFile;
            LogDelegate logger3 = LogToDatabase;

            // 使用Delegate.Combine合并委托
            LogDelegate multiLogger = (LogDelegate)Delegate.Combine(logger1, logger2, logger3);

            Console.WriteLine("调用合并后的日志委托:");
            multiLogger("这是一条重要日志");

            Console.WriteLine();

            // ========== 示例4：使用Delegate.Remove移除方法 ==========
            Console.WriteLine("【示例4：使用Delegate.Remove移除方法】");

            // 移除文件日志
            multiLogger = (LogDelegate)Delegate.Remove(multiLogger, logger2);

            Console.WriteLine("移除文件日志后:");
            multiLogger("这是另一条日志");

            Console.WriteLine();

            // ========== 示例5：多播委托的返回值问题 ==========
            Console.WriteLine("【示例5：多播委托的返回值（重要！）】");

            CalculationDelegate calc = Add;
            calc += Multiply;
            calc += Subtract;

            Console.WriteLine("\n调用多播委托（有返回值）:");
            int result = calc(10, 5);
            Console.WriteLine($"\n⚠️ 注意：多播委托只返回最后一个方法的返回值: {result}");
            Console.WriteLine("前面方法的返回值会被丢弃！");

            Console.WriteLine();

            // ========== 示例6：获取委托调用列表 ==========
            Console.WriteLine("【示例6：获取委托调用列表】");

            NotificationDelegate notifyChain = SendEmail;
            notifyChain += SendSMS;
            notifyChain += SendPushNotification;

            Console.WriteLine("委托链中的方法:");
            Delegate[] delegates = notifyChain.GetInvocationList();
            for (int i = 0; i < delegates.Length; i++)
            {
                Console.WriteLine($"  {i + 1}. {delegates[i].Method.Name}");
            }

            Console.WriteLine("\n手动调用每个委托并处理异常:");
            foreach (NotificationDelegate del in notifyChain.GetInvocationList())
            {
                try
                {
                    del("逐个调用的消息");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ 调用 {del.Method.Name} 时出错: {ex.Message}");
                }
            }

            Console.WriteLine();

            // ========== 示例7：多播委托的实际应用 - 事件系统 ==========
            Console.WriteLine("【示例7：实际应用 - 游戏事件系统】");

            GameEventSystem gameEvents = new GameEventSystem();

            // 订阅玩家死亡事件
            gameEvents.OnPlayerDeath += ShowDeathScreen;
            gameEvents.OnPlayerDeath += SaveGameData;
            gameEvents.OnPlayerDeath += SendDeathStatistics;

            // 触发事件
            Console.WriteLine("玩家死亡，触发事件:");
            gameEvents.TriggerPlayerDeath("玩家被怪物击败");

            Console.WriteLine();

            // ========== 示例8：委托的空值安全调用 ==========
            Console.WriteLine("【示例8：委托的空值安全调用】");

            NotificationDelegate emptyDelegate = null;

            // 错误做法（会抛出异常）
            // emptyDelegate("这会崩溃"); // NullReferenceException

            // 正确做法1：传统检查
            if (emptyDelegate != null)
            {
                emptyDelegate("不会执行");
            }
            Console.WriteLine("传统空值检查：委托为空，未执行");

            // 正确做法2：空值条件运算符（推荐）
            emptyDelegate?.Invoke("也不会执行");
            Console.WriteLine("使用?.运算符：安全调用");

            Console.WriteLine();

            // ========== 示例9：委托的组合运算 ==========
            Console.WriteLine("【示例9：委托的组合运算】");

            NotificationDelegate notify1 = SendEmail;
            NotificationDelegate notify2 = SendSMS;
            NotificationDelegate notify3 = SendPushNotification;

            // 使用 + 运算符组合
            NotificationDelegate combined = notify1 + notify2 + notify3;
            Console.WriteLine("组合三个委托:");
            combined("组合消息");

            // 使用 - 运算符移除
            NotificationDelegate reduced = combined - notify2;
            Console.WriteLine("\n移除中间的委托:");
            reduced("精简消息");

            Console.WriteLine();

            // ========== 示例10：多播委托的顺序 ==========
            Console.WriteLine("【示例10：多播委托的执行顺序】");

            LogDelegate orderedLog = null;
            orderedLog += (msg) => Console.WriteLine($"第1步: {msg}");
            orderedLog += (msg) => Console.WriteLine($"第2步: {msg}");
            orderedLog += (msg) => Console.WriteLine($"第3步: {msg}");

            Console.WriteLine("按添加顺序执行:");
            orderedLog("测试顺序");

            Console.WriteLine("\n========== 程序结束 ==========");
            Console.ReadKey();
        }

        // ========== 游戏事件系统示例类 ==========

        /// <summary>
        /// 游戏死亡事件处理：显示死亡界面
        /// </summary>
        static void ShowDeathScreen(string reason)
        {
            Console.WriteLine($"  💀 [UI] 显示死亡界面: {reason}");
        }

        /// <summary>
        /// 游戏死亡事件处理：保存游戏数据
        /// </summary>
        static void SaveGameData(string reason)
        {
            Console.WriteLine($"  💾 [存档] 保存游戏进度");
        }

        /// <summary>
        /// 游戏死亡事件处理：发送统计数据
        /// </summary>
        static void SendDeathStatistics(string reason)
        {
            Console.WriteLine($"  📊 [统计] 上报死亡数据: {reason}");
        }
    }

    /// <summary>
    /// 游戏事件系统类
    /// 演示多播委托在实际项目中的应用
    /// </summary>
    class GameEventSystem
    {
        // 定义玩家死亡事件委托
        public delegate void PlayerDeathDelegate(string reason);

        // 玩家死亡事件（多播委托）
        public PlayerDeathDelegate OnPlayerDeath;

        /// <summary>
        /// 触发玩家死亡事件
        /// </summary>
        public void TriggerPlayerDeath(string reason)
        {
            // 安全调用所有订阅者
            OnPlayerDeath?.Invoke(reason);
        }
    }
}
