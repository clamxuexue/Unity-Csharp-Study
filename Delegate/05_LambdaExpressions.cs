using System;
using System.Collections.Generic;
using System.Linq;

namespace LambdaExpressions
{
    /// <summary>
    /// 05 - Lambda表达式和匿名方法示例
    /// Lambda表达式是创建委托实例的简洁语法
    /// </summary>
    class LambdaExpressionsExample
    {
        // 委托声明
        public delegate int MathOperation(int a, int b);
        public delegate bool Condition(int value);
        public delegate void Printer(string message);

        static void Main(string[] args)
        {
            Console.WriteLine("========== Lambda表达式和匿名方法示例 ==========\n");

            // ========== 示例1：传统方法 vs 匿名方法 vs Lambda ==========
            Console.WriteLine("【示例1：三种方式对比】");

            // 方式1：传统命名方法
            Console.WriteLine("1. 传统命名方法:");
            MathOperation addMethod = Add;
            Console.WriteLine($"10 + 5 = {addMethod(10, 5)}");

            // 方式2：匿名方法（C# 2.0）
            Console.WriteLine("\n2. 匿名方法:");
            MathOperation addAnonymous = delegate (int a, int b)
            {
                return a + b;
            };
            Console.WriteLine($"10 + 5 = {addAnonymous(10, 5)}");

            // 方式3：Lambda表达式（C# 3.0+，推荐）
            Console.WriteLine("\n3. Lambda表达式:");
            MathOperation addLambda = (a, b) => a + b;
            Console.WriteLine($"10 + 5 = {addLambda(10, 5)}");

            Console.WriteLine();

            // ========== 示例2：Lambda表达式的各种形式 ==========
            Console.WriteLine("【示例2：Lambda表达式的各种形式】");

            // 形式1：无参数
            Action sayHello = () => Console.WriteLine("Hello!");
            sayHello();

            // 形式2：单个参数（可省略括号）
            Action<string> greet = name => Console.WriteLine($"你好, {name}!");
            greet("张三");

            // 形式3：多个参数（必须有括号）
            Func<int, int, int> multiply = (x, y) => x * y;
            Console.WriteLine($"3 × 4 = {multiply(3, 4)}");

            // 形式4：单行表达式（自动返回）
            Func<int, int> square = x => x * x;
            Console.WriteLine($"5的平方 = {square(5)}");

            // 形式5：多行语句块（需要显式return）
            Func<int, int, string> compare = (a, b) =>
            {
                if (a > b) return $"{a} 大于 {b}";
                else if (a < b) return $"{a} 小于 {b}";
                else return $"{a} 等于 {b}";
            };
            Console.WriteLine(compare(10, 5));

            // 形式6：显式类型声明（通常可省略）
            Func<int, int, int> divide = (int x, int y) => x / y;
            Console.WriteLine($"20 ÷ 4 = {divide(20, 4)}");

            Console.WriteLine();

            // ========== 示例3：Lambda捕获外部变量（闭包） ==========
            Console.WriteLine("【示例3：Lambda捕获外部变量（闭包）】");

            int multiplier = 3;
            Func<int, int> multiplyByFactor = x => x * multiplier;

            Console.WriteLine($"5 × {multiplier} = {multiplyByFactor(5)}");

            // 修改外部变量
            multiplier = 5;
            Console.WriteLine($"5 × {multiplier} = {multiplyByFactor(5)}");
            Console.WriteLine("注意：Lambda捕获的是变量引用，不是值！");

            Console.WriteLine();

            // ========== 示例4：Lambda在集合操作中的应用 ==========
            Console.WriteLine("【示例4：Lambda在集合操作中的应用】");

            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Console.WriteLine("原始数据: " + string.Join(", ", numbers));

            // Where：过滤
            var evenNumbers = numbers.Where(n => n % 2 == 0);
            Console.WriteLine("偶数: " + string.Join(", ", evenNumbers));

            // Select：转换
            var squared = numbers.Select(n => n * n);
            Console.WriteLine("平方: " + string.Join(", ", squared));

            // OrderBy：排序
            var descending = numbers.OrderByDescending(n => n);
            Console.WriteLine("降序: " + string.Join(", ", descending));

            // Any：判断是否存在
            bool hasLargeNumber = numbers.Any(n => n > 8);
            Console.WriteLine($"是否有大于8的数: {hasLargeNumber}");

            // All：判断是否全部满足
            bool allPositive = numbers.All(n => n > 0);
            Console.WriteLine($"是否全是正数: {allPositive}");

            // First：查找第一个
            int firstEven = numbers.First(n => n % 2 == 0);
            Console.WriteLine($"第一个偶数: {firstEven}");

            // Count：计数
            int countGreaterThan5 = numbers.Count(n => n > 5);
            Console.WriteLine($"大于5的数量: {countGreaterThan5}");

            // Sum：求和
            int sumOfEvens = numbers.Where(n => n % 2 == 0).Sum();
            Console.WriteLine($"偶数之和: {sumOfEvens}");

            Console.WriteLine();

            // ========== 示例5：复杂的Lambda表达式 ==========
            Console.WriteLine("【示例5：复杂的Lambda表达式】");

            List<Student> students = new List<Student>
            {
                new Student { Name = "张三", Age = 20, Score = 85 },
                new Student { Name = "李四", Age = 22, Score = 92 },
                new Student { Name = "王五", Age = 21, Score = 78 },
                new Student { Name = "赵六", Age = 23, Score = 95 },
                new Student { Name = "钱七", Age = 20, Score = 88 }
            };

            Console.WriteLine("学生列表:");
            students.ForEach(s => Console.WriteLine($"  {s.Name}, {s.Age}岁, 分数:{s.Score}"));

            // 查找成绩优秀的学生（分数>=90）
            Console.WriteLine("\n成绩优秀的学生:");
            var excellentStudents = students.Where(s => s.Score >= 90);
            excellentStudents.ToList().ForEach(s => Console.WriteLine($"  {s.Name}: {s.Score}分"));

            // 按分数排序
            Console.WriteLine("\n按分数排序:");
            var sortedByScore = students.OrderByDescending(s => s.Score);
            sortedByScore.ToList().ForEach(s => Console.WriteLine($"  {s.Name}: {s.Score}分"));

            // 分组统计
            Console.WriteLine("\n按年龄分组:");
            var groupedByAge = students.GroupBy(s => s.Age);
            foreach (var group in groupedByAge)
            {
                Console.WriteLine($"  {group.Key}岁: {string.Join(", ", group.Select(s => s.Name))}");
            }

            // 计算平均分
            double averageScore = students.Average(s => s.Score);
            Console.WriteLine($"\n平均分: {averageScore:F2}");

            Console.WriteLine();

            // ========== 示例6：Lambda表达式作为参数 ==========
            Console.WriteLine("【示例6：Lambda表达式作为参数】");

            int[] testData = { 5, 12, 8, 3, 15, 7, 20 };
            Console.WriteLine("测试数据: " + string.Join(", ", testData));

            // 传递不同的Lambda表达式实现不同的过滤
            Console.WriteLine("\n过滤大于10的数:");
            ProcessArray(testData, x => x > 10);

            Console.WriteLine("\n过滤偶数:");
            ProcessArray(testData, x => x % 2 == 0);

            Console.WriteLine("\n过滤5-15之间的数:");
            ProcessArray(testData, x => x >= 5 && x <= 15);

            Console.WriteLine();

            // ========== 示例7：Lambda表达式链式调用 ==========
            Console.WriteLine("【示例7：Lambda表达式链式调用】");

            var result = numbers
                .Where(n => n > 3)           // 过滤大于3的数
                .Select(n => n * 2)          // 乘以2
                .OrderByDescending(n => n)   // 降序排序
                .Take(5);                    // 取前5个

            Console.WriteLine("处理流程: 过滤>3 -> ×2 -> 降序 -> 取前5");
            Console.WriteLine("结果: " + string.Join(", ", result));

            Console.WriteLine();

            // ========== 示例8：匿名方法的特点 ==========
            Console.WriteLine("【示例8：匿名方法 vs Lambda】");

            // 匿名方法可以省略参数列表（如果不使用参数）
            Printer anonymousPrinter = delegate (string msg)
            {
                Console.WriteLine($"匿名方法: {msg}");
            };
            anonymousPrinter("测试消息");

            // Lambda更简洁
            Printer lambdaPrinter = msg => Console.WriteLine($"Lambda: {msg}");
            lambdaPrinter("测试消息");

            Console.WriteLine();

            // ========== 示例9：Lambda表达式的实际应用 ==========
            Console.WriteLine("【示例9：实际应用 - 数据验证】");

            DataValidator validator = new DataValidator();

            // 添加验证规则
            validator.AddRule(value => value > 0, "值必须大于0");
            validator.AddRule(value => value < 100, "值必须小于100");
            validator.AddRule(value => value % 2 == 0, "值必须是偶数");

            // 验证数据
            Console.WriteLine("验证 50:");
            validator.Validate(50);

            Console.WriteLine("\n验证 -5:");
            validator.Validate(-5);

            Console.WriteLine("\n验证 150:");
            validator.Validate(150);

            Console.WriteLine("\n验证 33:");
            validator.Validate(33);

            Console.WriteLine();

            // ========== 示例10：Lambda表达式的性能考虑 ==========
            Console.WriteLine("【示例10：Lambda表达式的性能考虑】");

            Console.WriteLine("❌ 不好的做法（每次循环都创建新委托）:");
            Console.WriteLine("for (int i = 0; i < 1000; i++)");
            Console.WriteLine("    SomeMethod(() => DoSomething());");

            Console.WriteLine("\n✅ 好的做法（缓存委托）:");
            Console.WriteLine("Action cachedAction = () => DoSomething();");
            Console.WriteLine("for (int i = 0; i < 1000; i++)");
            Console.WriteLine("    SomeMethod(cachedAction);");

            // 实际演示
            Action cachedLambda = () => { }; // 缓存的Lambda
            Console.WriteLine("\n在高频调用场景（如Unity的Update）中，应缓存Lambda表达式");

            Console.WriteLine("\n========== 程序结束 ==========");
            Console.ReadKey();
        }

        // ========== 辅助方法 ==========

        /// <summary>
        /// 传统的加法方法
        /// </summary>
        static int Add(int a, int b)
        {
            return a + b;
        }

        /// <summary>
        /// 处理数组并打印符合条件的元素
        /// </summary>
        static void ProcessArray(int[] array, Predicate<int> condition)
        {
            Console.Write("结果: ");
            foreach (int item in array)
            {
                if (condition(item))
                {
                    Console.Write($"{item} ");
                }
            }
            Console.WriteLine();
        }
    }

    // ========== 辅助类 ==========

    /// <summary>
    /// 学生类
    /// </summary>
    class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int Score { get; set; }
    }

    /// <summary>
    /// 数据验证器
    /// 演示Lambda表达式在验证场景中的应用
    /// </summary>
    class DataValidator
    {
        // 验证规则列表（规则函数，错误消息）
        private List<(Func<int, bool> rule, string message)> rules = new List<(Func<int, bool>, string)>();

        /// <summary>
        /// 添加验证规则
        /// </summary>
        public void AddRule(Func<int, bool> rule, string errorMessage)
        {
            rules.Add((rule, errorMessage));
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        public bool Validate(int value)
        {
            bool isValid = true;
            Console.WriteLine($"验证值: {value}");

            foreach (var (rule, message) in rules)
            {
                if (!rule(value))
                {
                    Console.WriteLine($"  ❌ {message}");
                    isValid = false;
                }
                else
                {
                    Console.WriteLine($"  ✅ 通过");
                }
            }

            return isValid;
        }
    }
}
