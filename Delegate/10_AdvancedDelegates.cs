using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedDelegates
{
    /// <summary>
    /// 10 - 委托高级用法示例
    /// 包含协变、逆变、委托缓存、异步调用等高级特性
    /// </summary>
    class AdvancedDelegatesExample
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== 委托高级用法示例 ==========\n");

            // ========== 示例1：委托的协变（Covariance） ==========
            Console.WriteLine("【示例1：委托的协变 - 返回值类型可以更具体】");

            // 协变：返回值类型可以是委托声明类型的派生类
            Func<Animal> animalFactory = GetDog; // Dog是Animal的派生类
            Animal animal = animalFactory();
            Console.WriteLine($"创建了: {animal.GetType().Name}");

            Console.WriteLine();

            // ========== 示例2：委托的逆变（Contravariance） ==========
            Console.WriteLine("【示例2：委托的逆变 - 参数类型可以更通用】");

            // 逆变：参数类型可以是委托声明类型的基类
            Action<Dog> dogAction = ProcessAnimal; // Animal是Dog的基类
            dogAction(new Dog { Name = "旺财" });

            Console.WriteLine();

            // ========== 示例3：委托缓存（性能优化） ==========
            Console.WriteLine("【示例3：委托缓存 - 避免重复创建】");

            Console.WriteLine("❌ 不好的做法（每次循环创建新委托）:");
            var timer1 = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 100000; i++)
            {
                // 每次都创建新的委托实例（产生GC压力）
                Action badAction = () => { };
            }
            timer1.Stop();
            Console.WriteLine($"耗时: {timer1.ElapsedMilliseconds}ms");

            Console.WriteLine("\n✅ 好的做法（缓存委托）:");
            var timer2 = System.Diagnostics.Stopwatch.StartNew();
            Action cachedAction = () => { }; // 缓存委托
            for (int i = 0; i < 100000; i++)
            {
                // 重用同一个委托实例
                Action goodAction = cachedAction;
            }
            timer2.Stop();
            Console.WriteLine($"耗时: {timer2.ElapsedMilliseconds}ms");

            Console.WriteLine();

            // ========== 示例4：多播委托的返回值 ==========
            Console.WriteLine("【示例4：多播委托的返回值问题】");

            Func<int> multiFunc = () => { Console.WriteLine("方法1返回10"); return 10; };
            multiFunc += () => { Console.WriteLine("方法2返回20"); return 20; };
            multiFunc += () => { Console.WriteLine("方法3返回30"); return 30; };

            int result = multiFunc();
            Console.WriteLine($"⚠️ 多播委托只返回最后一个方法的返回值: {result}");

            Console.WriteLine("\n解决方案：手动调用每个委托并收集返回值");
            List<int> results = new List<int>();
            foreach (Func<int> del in multiFunc.GetInvocationList())
            {
                results.Add(del());
            }
            Console.WriteLine($"所有返回值: {string.Join(", ", results)}");

            Console.WriteLine();

            // ========== 示例5：委托的异常处理 ==========
            Console.WriteLine("【示例5：多播委托的异常处理】");

            Action multiAction = () => Console.WriteLine("✅ 方法1执行成功");
            multiAction += () => { Console.WriteLine("❌ 方法2抛出异常"); throw new Exception("测试异常"); };
            multiAction += () => Console.WriteLine("⏭️ 方法3（不会执行）");

            Console.WriteLine("直接调用多播委托（异常会中断后续方法）:");
            try
            {
                multiAction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"捕获异常: {ex.Message}");
            }

            Console.WriteLine("\n安全调用方式（逐个调用并处理异常）:");
            foreach (Action del in multiAction.GetInvocationList())
            {
                try
                {
                    del();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"捕获异常: {ex.Message}，继续执行下一个");
                }
            }

            Console.WriteLine();

            // ========== 示例6：委托的比较和相等性 ==========
            Console.WriteLine("【示例6：委托的比较和相等性】");

            Action action1 = PrintMessage;
            Action action2 = PrintMessage;
            Action action3 = () => Console.WriteLine("Lambda");

            Console.WriteLine($"action1 == action2: {action1 == action2}"); // True（指向同一方法）
            Console.WriteLine($"action1 == action3: {action1 == action3}"); // False（不同方法）

            // 多播委托的比较
            Action multi1 = PrintMessage;
            multi1 += PrintHello;

            Action multi2 = PrintMessage;
            multi2 += PrintHello;

            Console.WriteLine($"multi1 == multi2: {multi1 == multi2}"); // True（相同的调用列表）

            Console.WriteLine();

            // ========== 示例7：委托与闭包 ==========
            Console.WriteLine("【示例7：委托与闭包（变量捕获）】");

            Console.WriteLine("⚠️ 常见陷阱：循环中的闭包");
            List<Action> actions = new List<Action>();

            // 错误示例
            for (int i = 0; i < 3; i++)
            {
                actions.Add(() => Console.WriteLine($"错误: i = {i}"));
            }

            Console.WriteLine("执行错误示例:");
            foreach (var action in actions)
            {
                action(); // 都会输出3，因为捕获的是变量引用
            }

            // 正确示例
            actions.Clear();
            for (int i = 0; i < 3; i++)
            {
                int captured = i; // 创建局部变量
                actions.Add(() => Console.WriteLine($"正确: captured = {captured}"));
            }

            Console.WriteLine("\n执行正确示例:");
            foreach (var action in actions)
            {
                action(); // 输出0, 1, 2
            }

            Console.WriteLine();

            // ========== 示例8：委托链的操作 ==========
            Console.WriteLine("【示例8：委托链的高级操作】");

            Action chain = Method1;
            chain += Method2;
            chain += Method3;

            Console.WriteLine("完整委托链:");
            chain();

            // 移除特定方法
            chain -= Method2;
            Console.WriteLine("\n移除Method2后:");
            chain();

            // 获取调用列表
            Console.WriteLine("\n委托链中的方法:");
            foreach (Delegate del in chain.GetInvocationList())
            {
                Console.WriteLine($"  - {del.Method.Name}");
            }

            Console.WriteLine();

            // ========== 示例9：委托的动态调用 ==========
            Console.WriteLine("【示例9：委托的动态调用】");

            // 使用DynamicInvoke
            Delegate dynamicDelegate = new Action<string>(PrintWithPrefix);
            Console.WriteLine("使用DynamicInvoke:");
            dynamicDelegate.DynamicInvoke("动态调用的消息");

            // 性能对比
            Action<string> typedDelegate = PrintWithPrefix;

            var sw1 = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 100000; i++)
            {
                typedDelegate("test");
            }
            sw1.Stop();

            var sw2 = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 100000; i++)
            {
                dynamicDelegate.DynamicInvoke("test");
            }
            sw2.Stop();

            Console.WriteLine($"\n性能对比（100000次调用）:");
            Console.WriteLine($"类型化调用: {sw1.ElapsedMilliseconds}ms");
            Console.WriteLine($"动态调用: {sw2.ElapsedMilliseconds}ms");
            Console.WriteLine("结论：动态调用慢很多，应避免在性能敏感的代码中使用");

            Console.WriteLine();

            // ========== 示例10：委托的实际应用 - 责任链模式 ==========
            Console.WriteLine("【示例10：责任链模式】");

            RequestHandler handler = new RequestHandler();

            // 添加处理器
            handler.AddHandler(ValidateRequest);
            handler.AddHandler(AuthenticateRequest);
            handler.AddHandler(ProcessRequest);
            handler.AddHandler(LogRequest);

            // 处理请求
            Request request = new Request { Data = "用户数据", IsValid = true };
            handler.Handle(request);

            Console.WriteLine();

            // ========== 示例11：委托的组合模式 ==========
            Console.WriteLine("【示例11：委托组合模式 - 管道处理】");

            // 创建数据处理管道
            Func<int, int> pipeline = x => x;
            pipeline = Compose(pipeline, x => x * 2);      // 乘以2
            pipeline = Compose(pipeline, x => x + 10);     // 加10
            pipeline = Compose(pipeline, x => x * x);      // 平方

            int input = 5;
            int output = pipeline(input);
            Console.WriteLine($"输入: {input}");
            Console.WriteLine($"处理流程: ×2 -> +10 -> 平方");
            Console.WriteLine($"输出: {output}"); // ((5*2)+10)^2 = 400

            Console.WriteLine();

            // ========== 示例12：委托的记忆化（Memoization） ==========
            Console.WriteLine("【示例12：委托的记忆化优化】");

            Func<int, int> fibonacci = null;
            fibonacci = n => n <= 1 ? n : fibonacci(n - 1) + fibonacci(n - 2);

            Console.WriteLine("未优化的斐波那契:");
            var sw3 = System.Diagnostics.Stopwatch.StartNew();
            int fib30 = fibonacci(30);
            sw3.Stop();
            Console.WriteLine($"fibonacci(30) = {fib30}, 耗时: {sw3.ElapsedMilliseconds}ms");

            // 使用记忆化优化
            var memoizedFib = Memoize<int, int>(fibonacci);

            Console.WriteLine("\n记忆化优化后:");
            var sw4 = System.Diagnostics.Stopwatch.StartNew();
            int memoFib30 = memoizedFib(30);
            sw4.Stop();
            Console.WriteLine($"fibonacci(30) = {memoFib30}, 耗时: {sw4.ElapsedMilliseconds}ms");

            Console.WriteLine();

            // ========== 示例13：委托的弱引用 ==========
            Console.WriteLine("【示例13：弱引用委托（防止内存泄漏）】");

            WeakReferenceExample weakRefExample = new WeakReferenceExample();
            weakRefExample.Demonstrate();

            Console.WriteLine("\n========== 程序结束 ==========");
            Console.ReadKey();
        }

        // ========== 辅助类和方法 ==========

        // 协变示例类
        class Animal { public string Name { get; set; } }
        class Dog : Animal { }

        static Dog GetDog() => new Dog { Name = "狗狗" };
        static void ProcessAnimal(Animal animal) => Console.WriteLine($"处理动物: {animal.Name}");

        // 简单方法
        static void PrintMessage() => Console.WriteLine("打印消息");
        static void PrintHello() => Console.WriteLine("Hello!");
        static void PrintWithPrefix(string msg) { /* 静默执行 */ }

        static void Method1() => Console.WriteLine("  执行Method1");
        static void Method2() => Console.WriteLine("  执行Method2");
        static void Method3() => Console.WriteLine("  执行Method3");

        // 责任链模式
        class Request
        {
            public string Data { get; set; }
            public bool IsValid { get; set; }
            public bool IsAuthenticated { get; set; }
        }

        static bool ValidateRequest(Request req)
        {
            Console.WriteLine("  1. 验证请求");
            return req.IsValid;
        }

        static bool AuthenticateRequest(Request req)
        {
            Console.WriteLine("  2. 认证请求");
            req.IsAuthenticated = true;
            return true;
        }

        static bool ProcessRequest(Request req)
        {
            Console.WriteLine("  3. 处理请求");
            return true;
        }

        static bool LogRequest(Request req)
        {
            Console.WriteLine("  4. 记录日志");
            return true;
        }

        class RequestHandler
        {
            private Func<Request, bool> handlerChain;

            public void AddHandler(Func<Request, bool> handler)
            {
                handlerChain += handler;
            }

            public void Handle(Request request)
            {
                if (handlerChain == null) return;

                foreach (Func<Request, bool> handler in handlerChain.GetInvocationList())
                {
                    if (!handler(request))
                    {
                        Console.WriteLine("  ❌ 处理中断");
                        return;
                    }
                }
                Console.WriteLine("  ✅ 请求处理完成");
            }
        }

        // 委托组合
        static Func<T, T> Compose<T>(Func<T, T> f, Func<T, T> g)
        {
            return x => g(f(x));
        }

        // 记忆化
        static Func<T, TResult> Memoize<T, TResult>(Func<T, TResult> func)
        {
            var cache = new Dictionary<T, TResult>();
            return arg =>
            {
                if (cache.ContainsKey(arg))
                    return cache[arg];

                TResult result = func(arg);
                cache[arg] = result;
                return result;
            };
        }

        // 弱引用示例
        class WeakReferenceExample
        {
            public void Demonstrate()
            {
                Console.WriteLine("创建对象并订阅事件...");
                Publisher publisher = new Publisher();
                Subscriber subscriber = new Subscriber();

                // 使用弱引用订阅
                WeakReference weakRef = new WeakReference(subscriber);
                publisher.OnEvent += subscriber.HandleEvent;

                Console.WriteLine("触发事件:");
                publisher.TriggerEvent();

                // 移除强引用
                subscriber = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();

                Console.WriteLine("\n对象是否存活: " + (weakRef.IsAlive ? "是" : "否"));
                Console.WriteLine("⚠️ 注意：如果不取消订阅，对象仍会存活（内存泄漏）");
            }

            class Publisher
            {
                public event Action OnEvent;
                public void TriggerEvent() => OnEvent?.Invoke();
            }

            class Subscriber
            {
                public void HandleEvent() => Console.WriteLine("  处理事件");
                ~Subscriber() => Console.WriteLine("  Subscriber被回收");
            }
        }
    }
}
