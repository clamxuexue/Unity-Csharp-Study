using System;

namespace DelegateUsage
{
    /// <summary>
    /// 02 - 委托的实际应用场景
    /// 本示例展示委托在实际开发中的常见用法
    /// </summary>
    class DelegateUsageExample
    {
        // ========== 委托声明 ==========

        /// <summary>
        /// 数据处理委托：用于处理整数数组
        /// </summary>
        public delegate void DataProcessor(int[] data);

        /// <summary>
        /// 数据过滤委托：用于判断数据是否符合条件
        /// </summary>
        public delegate bool DataFilter(int value);

        /// <summary>
        /// 数据转换委托：将一个类型转换为另一个类型
        /// </summary>
        public delegate string DataConverter(int value);

        // ========== 示例方法 ==========

        /// <summary>
        /// 打印数组中的所有元素
        /// </summary>
        public static void PrintArray(int[] data)
        {
            Console.Write("数组内容: [");
            for (int i = 0; i < data.Length; i++)
            {
                Console.Write(data[i]);
                if (i < data.Length - 1) Console.Write(", ");
            }
            Console.WriteLine("]");
        }

        /// <summary>
        /// 计算数组的总和
        /// </summary>
        public static void CalculateSum(int[] data)
        {
            int sum = 0;
            foreach (int num in data)
            {
                sum += num;
            }
            Console.WriteLine($"数组总和: {sum}");
        }

        /// <summary>
        /// 计算数组的平均值
        /// </summary>
        public static void CalculateAverage(int[] data)
        {
            if (data.Length == 0)
            {
                Console.WriteLine("数组为空，无法计算平均值");
                return;
            }

            int sum = 0;
            foreach (int num in data)
            {
                sum += num;
            }
            double average = (double)sum / data.Length;
            Console.WriteLine($"数组平均值: {average:F2}");
        }

        /// <summary>
        /// 判断是否为偶数
        /// </summary>
        public static bool IsEven(int value)
        {
            return value % 2 == 0;
        }

        /// <summary>
        /// 判断是否为正数
        /// </summary>
        public static bool IsPositive(int value)
        {
            return value > 0;
        }

        /// <summary>
        /// 判断是否大于10
        /// </summary>
        public static bool IsGreaterThanTen(int value)
        {
            return value > 10;
        }

        /// <summary>
        /// 将整数转换为二进制字符串
        /// </summary>
        public static string ToBinary(int value)
        {
            return Convert.ToString(value, 2);
        }

        /// <summary>
        /// 将整数转换为十六进制字符串
        /// </summary>
        public static string ToHex(int value)
        {
            return $"0x{value:X}";
        }

        // ========== 使用委托的工具方法 ==========

        /// <summary>
        /// 处理数据的通用方法
        /// 接受委托作为参数，实现不同的处理逻辑
        /// </summary>
        public static void ProcessData(int[] data, DataProcessor processor)
        {
            if (processor == null)
            {
                Console.WriteLine("处理器为空");
                return;
            }

            processor(data);
        }

        /// <summary>
        /// 过滤数组元素
        /// 根据传入的过滤条件返回符合条件的元素
        /// </summary>
        public static int[] FilterData(int[] data, DataFilter filter)
        {
            if (filter == null) return data;

            // 计算符合条件的元素数量
            int count = 0;
            foreach (int value in data)
            {
                if (filter(value)) count++;
            }

            // 创建结果数组
            int[] result = new int[count];
            int index = 0;
            foreach (int value in data)
            {
                if (filter(value))
                {
                    result[index++] = value;
                }
            }

            return result;
        }

        /// <summary>
        /// 转换数组元素
        /// 将整数数组转换为字符串数组
        /// </summary>
        public static string[] ConvertData(int[] data, DataConverter converter)
        {
            if (converter == null) return null;

            string[] result = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                result[i] = converter(data[i]);
            }

            return result;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("========== 委托实际应用示例 ==========\n");

            // 准备测试数据
            int[] numbers = { -5, 3, 8, -2, 15, 7, 12, -8, 20, 4 };

            // ========== 示例1：使用委托处理数据 ==========
            Console.WriteLine("【示例1：使用委托处理数据】");
            Console.WriteLine("原始数据:");
            PrintArray(numbers);
            Console.WriteLine();

            // 使用不同的处理器
            Console.WriteLine("使用打印处理器:");
            ProcessData(numbers, PrintArray);

            Console.WriteLine("\n使用求和处理器:");
            ProcessData(numbers, CalculateSum);

            Console.WriteLine("\n使用平均值处理器:");
            ProcessData(numbers, CalculateAverage);

            Console.WriteLine();

            // ========== 示例2：使用委托过滤数据 ==========
            Console.WriteLine("【示例2：使用委托过滤数据】");

            // 过滤偶数
            Console.WriteLine("过滤偶数:");
            int[] evenNumbers = FilterData(numbers, IsEven);
            PrintArray(evenNumbers);

            // 过滤正数
            Console.WriteLine("\n过滤正数:");
            int[] positiveNumbers = FilterData(numbers, IsPositive);
            PrintArray(positiveNumbers);

            // 过滤大于10的数
            Console.WriteLine("\n过滤大于10的数:");
            int[] largeNumbers = FilterData(numbers, IsGreaterThanTen);
            PrintArray(largeNumbers);

            Console.WriteLine();

            // ========== 示例3：使用委托转换数据 ==========
            Console.WriteLine("【示例3：使用委托转换数据】");

            int[] smallArray = { 10, 15, 20, 25 };
            Console.WriteLine("原始数据:");
            PrintArray(smallArray);

            // 转换为二进制
            Console.WriteLine("\n转换为二进制:");
            string[] binaryStrings = ConvertData(smallArray, ToBinary);
            Console.WriteLine($"[{string.Join(", ", binaryStrings)}]");

            // 转换为十六进制
            Console.WriteLine("\n转换为十六进制:");
            string[] hexStrings = ConvertData(smallArray, ToHex);
            Console.WriteLine($"[{string.Join(", ", hexStrings)}]");

            Console.WriteLine();

            // ========== 示例4：委托链式调用 ==========
            Console.WriteLine("【示例4：委托链式调用】");

            // 先过滤正数，再计算总和
            Console.WriteLine("处理流程：原始数据 -> 过滤正数 -> 计算总和");
            int[] filtered = FilterData(numbers, IsPositive);
            ProcessData(filtered, CalculateSum);

            Console.WriteLine();

            // ========== 示例5：委托存储和复用 ==========
            Console.WriteLine("【示例5：委托存储和复用】");

            // 将常用的委托存储起来
            DataProcessor sumProcessor = CalculateSum;
            DataProcessor avgProcessor = CalculateAverage;
            DataFilter evenFilter = IsEven;

            Console.WriteLine("对不同数据集使用相同的委托:");
            int[] dataset1 = { 1, 2, 3, 4, 5 };
            int[] dataset2 = { 10, 20, 30, 40, 50 };

            Console.WriteLine("\n数据集1:");
            ProcessData(dataset1, sumProcessor);
            ProcessData(dataset1, avgProcessor);

            Console.WriteLine("\n数据集2:");
            ProcessData(dataset2, sumProcessor);
            ProcessData(dataset2, avgProcessor);

            Console.WriteLine();

            // ========== 示例6：实际应用场景 - 回调模式 ==========
            Console.WriteLine("【示例6：回调模式】");

            Console.WriteLine("模拟异步操作完成后的回调:");
            PerformAsyncOperation(numbers, (data) =>
            {
                Console.WriteLine("异步操作完成！处理结果:");
                CalculateSum(data);
            });

            Console.WriteLine("\n========== 程序结束 ==========");
            Console.ReadKey();
        }

        /// <summary>
        /// 模拟异步操作
        /// 操作完成后通过委托回调通知调用者
        /// </summary>
        static void PerformAsyncOperation(int[] data, DataProcessor callback)
        {
            Console.WriteLine("开始异步操作...");
            // 模拟耗时操作
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("异步操作完成，执行回调...");

            // 执行回调
            callback?.Invoke(data);
        }
    }
}
