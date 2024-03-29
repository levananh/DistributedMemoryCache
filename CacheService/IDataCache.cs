﻿using System;
using System.Threading.Tasks;

namespace CacheService
{
    public interface IDataCache
    {
        /// <summary>
        /// Lưu dữ liệu vào cache
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu muốn lưu cache</typeparam>
        /// <param name="key">Key của cache</param>
        /// <param name="value">Giá trị lưu cache, dữ liệu truyền vào kiểu T</param>
        /// <param name="overrideExistedValue">
        /// true : Ghi đè lại giá trị của cache
        /// false hoặc null: nếu cache đã có dữ liệu rồi thì thôi không ghi đè (mặc định)
        /// </param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T value, bool? overrideExistedValue = null);

        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Trả về kết quả cache nếu có. Trong trường hợp cache ko có dữ liệu thì
        /// sẽ gọi hàm getDataForCaching
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="getDataForCaching"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key, Func<T> getDataForCaching);

        Task DeleteAsync(string key);

        Task RefreshAsync(string key);
    }
}
