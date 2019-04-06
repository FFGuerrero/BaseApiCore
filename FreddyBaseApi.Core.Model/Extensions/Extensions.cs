using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreddyBaseApi.Core.Model.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Get id value from entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int GetEntityKeyValue(this object entity)
        {
            var keyName = entity.GetType().GetProperties().FirstOrDefault(p => p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase)).Name;
            var keyValue = entity.GetType().GetProperty(keyName).GetValue(entity, null);

            if (keyValue == null)
                return 0;

            return int.Parse(keyValue.ToString());
        }

        /// <summary>
        /// Set value to entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static void SetEntityKeyValue(this object entity, object value)
        {
            var keyName = entity.GetType().GetProperties().FirstOrDefault(p => p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase)).Name;
            entity.GetType().GetProperty(keyName).SetValue(entity, value);
        }
    }
}
