using System;

namespace DelegateBasics
{
    /// <summary>
    /// 01 - 委托基础语法示例
    /// 本示例演示委托的最基本声明和使用方法
    /// </summary>
    class BasicDelegate
    {
        // ========== 第一步：声明委托类型 ==========
        // 委托声明语法：delegate 返回类型 委托名(参数列表);
        // 这里声明了一个无返回值、接受一个string参数的委托类型
        public delegate void PrintDelegate(string message);

        // 声明一个有返回值的委托类型
        public delegate int CalculateDelegate(int a, int b);

        // 声明一个无参数无返回值的委托类型
        public delegate void SimpleDelegate();

        // ========== 第二步：定义与委托签名匹配的方法 ==========

        /// <summary>
        /// 打印消息到控制台（匹配PrintDelegate签名）
        /// </summary>
        public static void PrintToConsole(string message)
        {
            Console.WriteLine($"控制台输出: {message}");
        }

        /// <summary>
        /// 打印大写消息（匹配PrintDelegate签名）
        /// </summary>
        public static void PrintUpperCase(string message)
        {
            Console.WriteLine($"大写输出: {message.ToUpper()}");
        }

        /// <summary>
        /// 加法运算（匹配CalculateDelegate签名）
        /// </summary>
        public static int Add(int a, int b)
        {
            int result = a + b;
            Console.WriteLine($"{a} + {b} = {result}");
            return result;
        }

        /// <summary>
        /// 乘法运算（匹配CalculateDelegate签名）
        /// </summary>
        public static int Multiply(int a, int b)
        {
            int result = a * b;
            Console.WriteLine($"{a} × {b} = {result}");
            return result;
        }

        /// <summary>
        /// 简单问候方法（匹配SimpleDelegate签名）
        /// </summary>
        public static void SayHello()
        {
            Console.WriteLine("你好，世界！");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("========== 委托基础示例 ==========\n");

            // ========== 示例1：基本委托使用 ==========
            Console.WriteLine("【示例1：基本委托使用】");

            // 创建委托实例，指向PrintToConsole方法
            PrintDelegate printDelegate = PrintToConsole;

            // 通过委托调用方法
            printDelegate("这是通过委托调用的消息");

            // 也可以使用Invoke方法显式调用
            printDelegate.Invoke("使用Invoke方法调用");

            Console.WriteLine();

            // ========== 示例2：委托重新赋值 ==========
            Console.WriteLine("【示例2：委托重新赋值】");

            // 将委托指向另一个方法
            printDelegate = PrintUpperCase;
            printDelegate("现在指向了不同的方法");

            Console.WriteLine();

            // ========== 示例3：有返回值的委托 ==========
            Console.WriteLine("【示例3：有返回值的委托】");

            // 创建计算委托，指向加法方法
            CalculateDelegate calcDelegate = Add;
            int sum = calcDelegate(10, 20);
            Console.WriteLine($"返回值: {sum}");

            // 改为指向乘法方法
            calcDelegate = Multiply;
            int product = calcDelegate(10, 20);
            Console.WriteLine($"返回值: {product}");

            Console.WriteLine();

            // ========== 示例4：无参数委托 ==========
            Console.WriteLine("【示例4：无参数委托】");

            SimpleDelegate simpleDelegate = SayHello;
            simpleDelegate();

            Console.WriteLine();

            // ========== 示例5：委托作为参数传递 ==========
            Console.WriteLine("【示例5：委托作为参数传递】");

            // 将不同的方法作为参数传递
            ExecutePrint(PrintToConsole, "通过参数传递的委托");
            ExecutePrint(PrintUpperCase, "另一个委托方法");

            Console.WriteLine();

            // ========== 示例6：委托的空值检查 ==========
            Console.WriteLine("【示例6：委托的空值检查】");

            PrintDelegate nullDelegate = null;

            // 错误的做法：直接调用可能导致NullReferenceException
            // nullDelegate("这会抛出异常"); // 不要这样做！

            // 正确的做法1：传统空值检查
            if (nullDelegate != null)
            {
                nullDelegate("这不会执行");
            }
            else
            {
                Console.WriteLine("委托为空，未执行");
            }

            // 正确的做法2：使用空值条件运算符（推荐）
            nullDelegate?.Invoke("这也不会执行");
            Console.WriteLine("使用?.运算符安全调用");

            Console.WriteLine();

            // ========== 示例7：委托与方法的对比 ==========
            Console.WriteLine("【示例7：委托与方法的对比】");

            // 直接调用方法
            Console.WriteLine("直接调用方法:");
            PrintToConsole("直接调用");

            // 通过委托调用
            Console.WriteLine("通过委托调用:");
            PrintDelegate delegateCall = PrintToConsole;
            delegateCall("委托调用");

            Console.WriteLine("\n委托的优势：可以作为参数传递、存储、动态改变行为");

            Console.WriteLine("\n========== 程序结束 ==========");
            Console.ReadKey();
        }

        /// <summary>
        /// 接受委托作为参数的方法
        /// 这展示了委托的强大之处：可以将行为作为参数传递
        /// </summary>
        /// <param name="printMethod">打印方法的委托</param>
        /// <param name="message">要打印的消息</param>
        static void ExecutePrint(PrintDelegate printMethod, string message)
        {
            Console.WriteLine("准备执行传入的打印方法...");
            printMethod(message);
        }
    }
}
