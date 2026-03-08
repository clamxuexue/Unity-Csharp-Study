using System;
using System.Collections.Generic;

namespace BuiltInDelegates
{
    /// <summary>
    /// 04 - 内置委托类型示例
    /// C#提供了三种常用的内置委托：Action、Func、Predicate
    /// 使用内置委托可以避免声明自定义委托类型
    /// </summary>
    class BuiltInDelegatesExample
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== 内置委托类型示例 ==========\n");

            // ========== 示例1：Action委托（无返回值） ==========
            Console.WriteLine("【示例1：Action委托 - 无返回值】");
            Console.WriteLine("Action是无返回值的委托，可以有0-16个参数\n");

            // Action：无参数
            Action simpleAction = () => Console.WriteLine("执行无参数Action");
            simpleAction();

            // Action<T>：一个参数
            Action<string> printAction = (message) => Console.WriteLine($"消息: {message}");
            printAction("Hello Action!");

            // Action<T1, T2>：两个参数
            Action<string, int> printWithNumber = (text, number) =>
            {
                Console.WriteLine($"{text}: {number}");
            };
            printWithNumber("数字是", 42);

            // Action<T1, T2, T3>：三个参数
            Action<int, int, int> printSum = (a, b, c) =>
            {
                Console.WriteLine($"{a} + {b} + {c} = {a + b + c}");
            };
            printSum(10, 20, 30);

            Console.WriteLine();

            // ========== 示例2：Func委托（有返回值） ==========
            Console.WriteLine("【示例2：Func委托 - 有返回值】");
            Console.WriteLine("Func的最后一个类型参数是返回值类型\n");

            // Func<TResult>：无参数，有返回值
            Func<int> getRandomNumber = () =>
            {
                return new Random().Next(1, 100);
            };
            Console.WriteLine($"随机数: {getRandomNumber()}");

            // Func<T, TResult>：一个参数，有返回值
            Func<int, int> square = (x) => x * x;
            Console.WriteLine($"5的平方: {square(5)}");

            // Func<T1, T2, TResult>：两个参数，有返回值
            Func<int, int, int> add = (a, b) => a + b;
            Console.WriteLine($"10 + 20 = {add(10, 20)}");

            // Func<T1, T2, T3, TResult>：三个参数，有返回值
            Func<string, string, string, string> combineStrings = (s1, s2, s3) =>
            {
                return $"{s1} {s2} {s3}";
            };
            Console.WriteLine($"组合字符串: {combineStrings("Hello", "C#", "World")}");

            Console.WriteLine();

            // ========== 示例3：Predicate委托（返回bool） ==========
            Console.WriteLine("【示例3：Predicate委托 - 返回bool】");
            Console.WriteLine("Predicate用于条件判断，只接受一个参数，返回bool\n");

            // Predicate<T>：判断条件
            Predicate<int> isEven = (number) => number % 2 == 0;
            Console.WriteLine($"4是偶数吗? {isEven(4)}");
            Console.WriteLine($"7是偶数吗? {isEven(7)}");

            Predicate<string> isLongString = (str) => str.Length > 5;
            Console.WriteLine($"'Hello'是长字符串吗? {isLongString("Hello")}");
            Console.WriteLine($"'Hello World'是长字符串吗? {isLongString("Hello World")}");

            Console.WriteLine();

            // ========== 示例4：Action的实际应用 ==========
            Console.WriteLine("【示例4：Action的实际应用 - 日志系统】");

            Logger logger = new Logger();

            // 使用Action作为日志处理器
            logger.AddLogHandler((message) => Console.WriteLine($"[控制台] {message}"));
            logger.AddLogHandler((message) => Console.WriteLine($"[文件] 写入: {message}"));

            logger.Log("应用程序启动");
            logger.Log("用户登录成功");

            Console.WriteLine();

            // ========== 示例5：Func的实际应用 ==========
            Console.WriteLine("【示例5：Func的实际应用 - 数据转换】");

            DataProcessor processor = new DataProcessor();

            // 使用Func进行数据转换
            int[] numbers = { 1, 2, 3, 4, 5 };

            Console.WriteLine("原始数据: " + string.Join(", ", numbers));

            // 转换为平方
            int[] squared = processor.Transform(numbers, x => x * x);
            Console.WriteLine("平方后: " + string.Join(", ", squared));

            // 转换为字符串
            string[] strings = processor.TransformToString(numbers, x => $"数字{x}");
            Console.WriteLine("转换为字符串: " + string.Join(", ", strings));

            Console.WriteLine();

            // ========== 示例6：Predicate的实际应用 ==========
            Console.WriteLine("【示例6：Predicate的实际应用 - 数据过滤】");

            int[] testNumbers = { -5, 3, 8, -2, 15, 7, 12, -8, 20, 4 };
            Console.WriteLine("原始数据: " + string.Join(", ", testNumbers));

            // 过滤正数
            int[] positives = processor.Filter(testNumbers, x => x > 0);
            Console.WriteLine("正数: " + string.Join(", ", positives));

            // 过滤偶数
            int[] evens = processor.Filter(testNumbers, x => x % 2 == 0);
            Console.WriteLine("偶数: " + string.Join(", ", evens));

            // 过滤大于10的数
            int[] largeNumbers = processor.Filter(testNumbers, x => x > 10);
            Console.WriteLine("大于10: " + string.Join(", ", largeNumbers));

            Console.WriteLine();

            // ========== 示例7：组合使用内置委托 ==========
            Console.WriteLine("【示例7：组合使用内置委托】");

            Calculator calculator = new Calculator();

            // 使用Func定义运算
            Func<int, int, int> addOp = (a, b) => a + b;
            Func<int, int, int> multiplyOp = (a, b) => a * b;

            // 使用Action输出结果
            Action<string, int> printResult = (operation, result) =>
            {
                Console.WriteLine($"{operation}的结果: {result}");
            };

            int result1 = calculator.Calculate(10, 5, addOp);
            printResult("加法", result1);

            int result2 = calculator.Calculate(10, 5, multiplyOp);
            printResult("乘法", result2);

            Console.WriteLine();

            // ========== 示例8：内置委托 vs 自定义委托 ==========
            Console.WriteLine("【示例8：内置委托 vs 自定义委托】");

            // 自定义委托方式（不推荐）
            Console.WriteLine("使用自定义委托:");
            CustomDelegateExample.ProcessDelegate customProcess = CustomDelegateExample.PrintMessage;
            customProcess("使用自定义委托");

            // 内置委托方式（推荐）
            Console.WriteLine("\n使用内置委托:");
            Action<string> builtInProcess = (msg) => Console.WriteLine($"消息: {msg}");
            builtInProcess("使用内置委托 - 更简洁！");

            Console.WriteLine();

            // ========== 示例9：高级应用 - 策略模式 ==========
            Console.WriteLine("【示例9：高级应用 - 策略模式】");

            PriceCalculator priceCalc = new PriceCalculator();

            // 定义不同的折扣策略
            Func<double, double> noDiscount = price => price;
            Func<double, double> tenPercent = price => price * 0.9;
            Func<double, double> twentyPercent = price => price * 0.8;
            Func<double, double> halfPrice = price => price * 0.5;

            double originalPrice = 100.0;

            Console.WriteLine($"原价: {originalPrice:C}");
            Console.WriteLine($"无折扣: {priceCalc.CalculatePrice(originalPrice, noDiscount):C}");
            Console.WriteLine($"9折: {priceCalc.CalculatePrice(originalPrice, tenPercent):C}");
            Console.WriteLine($"8折: {priceCalc.CalculatePrice(originalPrice, twentyPercent):C}");
            Console.WriteLine($"半价: {priceCalc.CalculatePrice(originalPrice, halfPrice):C}");

            Console.WriteLine();

            // ========== 示例10：内置委托的多播 ==========
            Console.WriteLine("【示例10：内置委托的多播】");

            Action<string> multiAction = null;
            multiAction += msg => Console.WriteLine($"处理器1: {msg}");
            multiAction += msg => Console.WriteLine($"处理器2: {msg}");
            multiAction += msg => Console.WriteLine($"处理器3: {msg}");

            Console.WriteLine("调用多播Action:");
            multiAction("多播消息");

            Console.WriteLine("\n========== 程序结束 ==========");
            Console.ReadKey();
        }
    }

    // ========== 辅助类 ==========

    /// <summary>
    /// 日志系统类 - 演示Action的应用
    /// </summary>
    class Logger
    {
        private Action<string> logHandlers;

        /// <summary>
        /// 添加日志处理器
        /// </summary>
        public void AddLogHandler(Action<string> handler)
        {
            logHandlers += handler;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        public void Log(string message)
        {
            logHandlers?.Invoke($"[{DateTime.Now:HH:mm:ss}] {message}");
        }
    }

    /// <summary>
    /// 数据处理器类 - 演示Func和Predicate的应用
    /// </summary>
    class DataProcessor
    {
        /// <summary>
        /// 转换数据（int到int）
        /// </summary>
        public int[] Transform(int[] data, Func<int, int> transformer)
        {
            int[] result = new int[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                result[i] = transformer(data[i]);
            }
            return result;
        }

        /// <summary>
        /// 转换数据（int到string）
        /// </summary>
        public string[] TransformToString(int[] data, Func<int, string> transformer)
        {
            string[] result = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                result[i] = transformer(data[i]);
            }
            return result;
        }

        /// <summary>
        /// 过滤数据
        /// </summary>
        public int[] Filter(int[] data, Predicate<int> predicate)
        {
            List<int> result = new List<int>();
            foreach (int item in data)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }
            return result.ToArray();
        }
    }

    /// <summary>
    /// 计算器类 - 演示Func的应用
    /// </summary>
    class Calculator
    {
        /// <summary>
        /// 执行计算
        /// </summary>
        public int Calculate(int a, int b, Func<int, int, int> operation)
        {
            return operation(a, b);
        }
    }

    /// <summary>
    /// 价格计算器 - 演示策略模式
    /// </summary>
    class PriceCalculator
    {
        /// <summary>
        /// 根据策略计算价格
        /// </summary>
        public double CalculatePrice(double originalPrice, Func<double, double> discountStrategy)
        {
            return discountStrategy(originalPrice);
        }
    }

    /// <summary>
    /// 自定义委托示例（对比用）
    /// </summary>
    class CustomDelegateExample
    {
        // 自定义委托（不推荐，应使用Action<string>）
        public delegate void ProcessDelegate(string message);

        public static void PrintMessage(string message)
        {
            Console.WriteLine($"消息: {message}");
        }
    }
}
