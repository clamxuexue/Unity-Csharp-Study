using System;

namespace EventsExample
{
    /// <summary>
    /// 06 - 事件（Event）示例
    /// 事件是基于委托的特殊封装，用于实现发布-订阅模式
    /// </summary>
    class EventsExample
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== 事件（Event）示例 ==========\n");

            // ========== 示例1：事件 vs 委托的区别 ==========
            Console.WriteLine("【示例1：事件 vs 委托的区别】");

            DelegateExample delegateObj = new DelegateExample();
            EventExample eventObj = new EventExample();

            // 委托可以在外部直接赋值（不安全）
            delegateObj.OnNotify = () => Console.WriteLine("委托被外部覆盖了！");
            delegateObj.Notify();

            // 事件只能使用 += 和 -= （安全）
            // eventObj.OnNotify = () => { }; // 编译错误！事件不能直接赋值
            eventObj.OnNotify += () => Console.WriteLine("事件订阅1");
            eventObj.OnNotify += () => Console.WriteLine("事件订阅2");
            eventObj.Notify();

            Console.WriteLine("\n结论：事件提供了更好的封装性，防止外部代码破坏订阅列表");

            Console.WriteLine();

            // ========== 示例2：标准事件模式 ==========
            Console.WriteLine("【示例2：标准事件模式】");

            Button button = new Button("确认按钮");

            // 订阅事件
            button.Click += Button_OnClick;
            button.Click += Button_OnClickLog;

            // 触发事件
            button.PerformClick();

            Console.WriteLine();

            // ========== 示例3：自定义事件参数 ==========
            Console.WriteLine("【示例3：自定义事件参数】");

            Player player = new Player("勇士");

            // 订阅玩家事件
            player.HealthChanged += Player_OnHealthChanged;
            player.LevelUp += Player_OnLevelUp;
            player.Died += Player_OnDied;

            // 触发各种事件
            player.TakeDamage(30);
            player.Heal(20);
            player.GainExperience(100);
            player.TakeDamage(100);

            Console.WriteLine();

            // ========== 示例4：事件的订阅和取消订阅 ==========
            Console.WriteLine("【示例4：事件的订阅和取消订阅】");

            Timer timer = new Timer();

            // 订阅事件
            timer.Tick += Timer_OnTick;
            timer.Tick += Timer_OnTickWithMessage;

            Console.WriteLine("启动计时器（2次tick）:");
            timer.Start(2);

            // 取消订阅
            timer.Tick -= Timer_OnTickWithMessage;

            Console.WriteLine("\n取消一个订阅后（2次tick）:");
            timer.Start(2);

            Console.WriteLine();

            // ========== 示例5：事件的实际应用 - 观察者模式 ==========
            Console.WriteLine("【示例5：观察者模式 - 股票价格监控】");

            Stock stock = new Stock("AAPL", 150.0);

            // 多个观察者订阅股票价格变化
            StockObserver investor1 = new StockObserver("投资者A");
            StockObserver investor2 = new StockObserver("投资者B");
            StockObserver investor3 = new StockObserver("投资者C");

            stock.PriceChanged += investor1.OnPriceChanged;
            stock.PriceChanged += investor2.OnPriceChanged;
            stock.PriceChanged += investor3.OnPriceChanged;

            // 价格变化
            stock.UpdatePrice(155.0);
            stock.UpdatePrice(148.0);

            // 投资者B取消订阅
            Console.WriteLine("\n投资者B取消订阅");
            stock.PriceChanged -= investor2.OnPriceChanged;

            stock.UpdatePrice(160.0);

            Console.WriteLine();

            // ========== 示例6：事件访问器（高级） ==========
            Console.WriteLine("【示例6：自定义事件访问器】");

            CustomEventExample customEvent = new CustomEventExample();

            customEvent.DataReceived += (data) => Console.WriteLine($"接收到数据: {data}");
            customEvent.DataReceived += (data) => Console.WriteLine($"处理数据: {data}");

            customEvent.TriggerEvent("测试数据");

            Console.WriteLine();

            // ========== 示例7：事件的线程安全 ==========
            Console.WriteLine("【示例7：事件的线程安全调用】");

            SafeEventExample safeEvent = new SafeEventExample();

            safeEvent.StatusChanged += (status) => Console.WriteLine($"状态变更: {status}");

            safeEvent.ChangeStatus("运行中");
            safeEvent.ChangeStatus("已停止");

            Console.WriteLine();

            // ========== 示例8：事件与内存泄漏 ==========
            Console.WriteLine("【示例8：事件与内存泄漏预防】");

            Publisher publisher = new Publisher();

            // 创建订阅者
            Subscriber subscriber1 = new Subscriber("订阅者1");
            subscriber1.Subscribe(publisher);

            publisher.PublishMessage("第一条消息");

            // 正确的做法：取消订阅
            subscriber1.Unsubscribe(publisher);
            Console.WriteLine("订阅者1已取消订阅");

            publisher.PublishMessage("第二条消息（订阅者1不会收到）");

            Console.WriteLine("\n⚠️ 重要：如果不取消订阅，即使subscriber1不再使用，");
            Console.WriteLine("   publisher仍持有其引用，导致内存泄漏！");

            Console.WriteLine();

            // ========== 示例9：事件的最佳实践 ==========
            Console.WriteLine("【示例9：事件的最佳实践】");

            BestPracticeExample example = new BestPracticeExample();

            example.DataProcessed += (sender, e) =>
            {
                Console.WriteLine($"数据处理完成: {e.Data}, 耗时: {e.ProcessTime}ms");
            };

            example.ProcessData("示例数据");

            Console.WriteLine();

            // ========== 示例10：Unity风格的事件 ==========
            Console.WriteLine("【示例10：Unity风格的事件（UnityAction）】");

            GameManager gameManager = new GameManager();

            // 订阅游戏事件
            gameManager.OnGameStart += () => Console.WriteLine("🎮 游戏开始！");
            gameManager.OnGameStart += () => Console.WriteLine("📊 加载玩家数据");
            gameManager.OnGameStart += () => Console.WriteLine("🎵 播放背景音乐");

            gameManager.OnGameOver += (score) => Console.WriteLine($"🏁 游戏结束！得分: {score}");

            // 触发事件
            gameManager.StartGame();
            gameManager.EndGame(1000);

            Console.WriteLine("\n========== 程序结束 ==========");
            Console.ReadKey();
        }

        // ========== 事件处理方法 ==========

        static void Button_OnClick(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Console.WriteLine($"  按钮 '{btn?.Name}' 被点击了！");
        }

        static void Button_OnClickLog(object sender, EventArgs e)
        {
            Console.WriteLine($"  [日志] 记录按钮点击事件");
        }

        static void Player_OnHealthChanged(object sender, HealthChangedEventArgs e)
        {
            Console.WriteLine($"  💚 生命值变化: {e.OldHealth} -> {e.NewHealth} (变化: {e.Change:+#;-#;0})");
        }

        static void Player_OnLevelUp(object sender, LevelUpEventArgs e)
        {
            Console.WriteLine($"  ⭐ 升级！等级: {e.OldLevel} -> {e.NewLevel}");
        }

        static void Player_OnDied(object sender, EventArgs e)
        {
            Player player = sender as Player;
            Console.WriteLine($"  💀 {player?.Name} 死亡了！");
        }

        static void Timer_OnTick(object sender, EventArgs e)
        {
            Console.WriteLine("  ⏰ Tick!");
        }

        static void Timer_OnTickWithMessage(object sender, EventArgs e)
        {
            Console.WriteLine("  📢 计时器触发");
        }
    }

    // ========== 示例类：委托 vs 事件 ==========

    class DelegateExample
    {
        public Action OnNotify; // 普通委托

        public void Notify()
        {
            OnNotify?.Invoke();
        }
    }

    class EventExample
    {
        public event Action OnNotify; // 事件

        public void Notify()
        {
            OnNotify?.Invoke();
        }
    }

    // ========== 示例类：标准事件模式 ==========

    class Button
    {
        public string Name { get; private set; }

        // 标准事件声明：使用EventHandler
        public event EventHandler Click;

        public Button(string name)
        {
            Name = name;
        }

        public void PerformClick()
        {
            Console.WriteLine($"执行点击: {Name}");
            // 触发事件（标准模式）
            OnClick(EventArgs.Empty);
        }

        // 受保护的虚方法，用于触发事件
        protected virtual void OnClick(EventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }

    // ========== 示例类：自定义事件参数 ==========

    /// <summary>
    /// 生命值变化事件参数
    /// </summary>
    class HealthChangedEventArgs : EventArgs
    {
        public int OldHealth { get; set; }
        public int NewHealth { get; set; }
        public int Change => NewHealth - OldHealth;
    }

    /// <summary>
    /// 升级事件参数
    /// </summary>
    class LevelUpEventArgs : EventArgs
    {
        public int OldLevel { get; set; }
        public int NewLevel { get; set; }
    }

    /// <summary>
    /// 玩家类
    /// </summary>
    class Player
    {
        public string Name { get; private set; }
        private int health = 100;
        private int level = 1;
        private int experience = 0;

        // 定义事件
        public event EventHandler<HealthChangedEventArgs> HealthChanged;
        public event EventHandler<LevelUpEventArgs> LevelUp;
        public event EventHandler Died;

        public Player(string name)
        {
            Name = name;
        }

        public void TakeDamage(int damage)
        {
            int oldHealth = health;
            health = Math.Max(0, health - damage);

            HealthChanged?.Invoke(this, new HealthChangedEventArgs
            {
                OldHealth = oldHealth,
                NewHealth = health
            });

            if (health == 0)
            {
                Died?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Heal(int amount)
        {
            int oldHealth = health;
            health = Math.Min(100, health + amount);

            HealthChanged?.Invoke(this, new HealthChangedEventArgs
            {
                OldHealth = oldHealth,
                NewHealth = health
            });
        }

        public void GainExperience(int exp)
        {
            experience += exp;
            if (experience >= 100)
            {
                int oldLevel = level;
                level++;
                experience = 0;

                LevelUp?.Invoke(this, new LevelUpEventArgs
                {
                    OldLevel = oldLevel,
                    NewLevel = level
                });
            }
        }
    }

    // ========== 示例类：计时器 ==========

    class Timer
    {
        public event EventHandler Tick;

        public void Start(int ticks)
        {
            for (int i = 0; i < ticks; i++)
            {
                Tick?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    // ========== 示例类：观察者模式 ==========

    class PriceChangedEventArgs : EventArgs
    {
        public string Symbol { get; set; }
        public double OldPrice { get; set; }
        public double NewPrice { get; set; }
        public double Change => NewPrice - OldPrice;
        public double ChangePercent => (Change / OldPrice) * 100;
    }

    class Stock
    {
        public string Symbol { get; private set; }
        private double price;

        public event EventHandler<PriceChangedEventArgs> PriceChanged;

        public Stock(string symbol, double initialPrice)
        {
            Symbol = symbol;
            price = initialPrice;
        }

        public void UpdatePrice(double newPrice)
        {
            double oldPrice = price;
            price = newPrice;

            PriceChanged?.Invoke(this, new PriceChangedEventArgs
            {
                Symbol = Symbol,
                OldPrice = oldPrice,
                NewPrice = newPrice
            });
        }
    }

    class StockObserver
    {
        public string Name { get; private set; }

        public StockObserver(string name)
        {
            Name = name;
        }

        public void OnPriceChanged(object sender, PriceChangedEventArgs e)
        {
            string trend = e.Change > 0 ? "📈" : "📉";
            Console.WriteLine($"  {trend} [{Name}] {e.Symbol}: ${e.OldPrice:F2} -> ${e.NewPrice:F2} ({e.ChangePercent:+0.00;-0.00}%)");
        }
    }

    // ========== 示例类：自定义事件访问器 ==========

    class CustomEventExample
    {
        private event Action<string> dataReceived;

        public event Action<string> DataReceived
        {
            add
            {
                Console.WriteLine("  [添加订阅者]");
                dataReceived += value;
            }
            remove
            {
                Console.WriteLine("  [移除订阅者]");
                dataReceived -= value;
            }
        }

        public void TriggerEvent(string data)
        {
            dataReceived?.Invoke(data);
        }
    }

    // ========== 示例类：线程安全事件 ==========

    class SafeEventExample
    {
        public event Action<string> StatusChanged;

        public void ChangeStatus(string newStatus)
        {
            // 线程安全的事件调用方式
            Action<string> handler = StatusChanged;
            handler?.Invoke(newStatus);
        }
    }

    // ========== 示例类：内存泄漏预防 ==========

    class Publisher
    {
        public event Action<string> MessagePublished;

        public void PublishMessage(string message)
        {
            Console.WriteLine($"发布消息: {message}");
            MessagePublished?.Invoke(message);
        }
    }

    class Subscriber
    {
        public string Name { get; private set; }

        public Subscriber(string name)
        {
            Name = name;
        }

        public void Subscribe(Publisher publisher)
        {
            publisher.MessagePublished += OnMessageReceived;
        }

        public void Unsubscribe(Publisher publisher)
        {
            publisher.MessagePublished -= OnMessageReceived;
        }

        private void OnMessageReceived(string message)
        {
            Console.WriteLine($"  [{Name}] 收到消息: {message}");
        }
    }

    // ========== 示例类：最佳实践 ==========

    class DataProcessedEventArgs : EventArgs
    {
        public string Data { get; set; }
        public int ProcessTime { get; set; }
    }

    class BestPracticeExample
    {
        // 使用泛型EventHandler<T>
        public event EventHandler<DataProcessedEventArgs> DataProcessed;

        public void ProcessData(string data)
        {
            // 模拟数据处理
            int processTime = 100;

            // 触发事件
            OnDataProcessed(new DataProcessedEventArgs
            {
                Data = data,
                ProcessTime = processTime
            });
        }

        // 受保护的虚方法
        protected virtual void OnDataProcessed(DataProcessedEventArgs e)
        {
            DataProcessed?.Invoke(this, e);
        }
    }

    // ========== 示例类：Unity风格事件 ==========

    class GameManager
    {
        // Unity风格：使用Action作为事件
        public event Action OnGameStart;
        public event Action<int> OnGameOver;

        public void StartGame()
        {
            Console.WriteLine("\n启动游戏...");
            OnGameStart?.Invoke();
        }

        public void EndGame(int finalScore)
        {
            Console.WriteLine("\n游戏结束...");
            OnGameOver?.Invoke(finalScore);
        }
    }
}
