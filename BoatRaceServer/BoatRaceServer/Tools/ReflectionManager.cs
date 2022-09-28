using System;
using System.Reflection;

namespace BoatRaceServer.Tools
{
    public class ReflectionManager : Singleton<ReflectionManager>
    {
        ReflectionManager() { }

        /// <summary>
        /// 创建实例
        /// ConfigData configData = ReflectionManager.Instance.CreateInstance<ConfigData>("BoatRaceServer.Tools.ConfigData");
        /// </summary>
        /// <param name="name">类型全名</param>
        public T CreateInstance<T>(string name)
        {
            // 获取类型
            var type = typeof(T).Assembly.GetType(name);
            // 创建实例对象
            object instance = Activator.CreateInstance(type);
            return (T)instance;
        }
        
        #region 设置属性值

        public bool SetPublicValue(string fieldName, object value, object instance)
        {
            return SetModelValue(fieldName, value, instance, BindingFlags.Public | BindingFlags.Instance);
        }
        
        public bool SetNonPublicValue(string fieldName, object value, object instance)
        {
            return SetModelValue(fieldName, value, instance, BindingFlags.NonPublic | BindingFlags.Instance);
        }
        
        private bool SetModelValue(string fieldName, object value, object instance, BindingFlags bindingAttr)
        {
            try
            {
                Type type = instance.GetType();
                FieldInfo fieldInfo = type.GetField(fieldName, bindingAttr);
                object v = Convert.ChangeType(value, fieldInfo.FieldType);
                fieldInfo.SetValue(instance, v);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
        

        #region 获取属性值
        
        public T GetPublicModelValue<T>(string fieldName, object instance)
        {
            return GetModelValue<T>(fieldName, instance, BindingFlags.Public | BindingFlags.Instance);
        }

        public T GetNonPublicModelValue<T>(string fieldName, object instance)
        {
            return GetModelValue<T>(fieldName, instance, BindingFlags.NonPublic | BindingFlags.Instance);   
        }
        
        private T GetModelValue<T>(string fieldName, object instance, BindingFlags bindingAttr)
        {
            try
            {
                Type type = instance.GetType();
                FieldInfo fieldInfo = type.GetField(fieldName, bindingAttr);
                return (T)fieldInfo.GetValue(instance);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        #endregion

        #region 执行方法

        public T CallPublicMethod<T>(string methodName, object instance, object[] parameters)
        {
            return CallMethod<T>(methodName, instance, BindingFlags.Public | BindingFlags.Instance, parameters);
        }

        public T CallNonPublicMethod<T>(string methodName, object instance, object[] parameters)
        {
            return CallMethod<T>(methodName, instance, BindingFlags.NonPublic | BindingFlags.Instance, parameters);
        }
        
        private T CallMethod<T>(string methodName, object instance, BindingFlags bindingAttr, object[] parameters)
        {
            try
            {
                Type type = instance.GetType();
                MethodInfo methodInfo = type.GetMethod(methodName, bindingAttr);
                var result = methodInfo.Invoke(instance, parameters);
                return (T) result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion
    }
}