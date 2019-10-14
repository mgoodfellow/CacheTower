﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheTower.Providers.Redis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;

namespace CacheTower.Tests.Providers.Redis
{
	[TestClass, Ignore]
	public class RedisCacheLayerTests : BaseCacheLayerTests
	{
		private ConnectionMultiplexer Connection { get; set; }

		[TestInitialize]
		public async Task Setup()
		{
			if (Connection == null)
			{
				var config = new ConfigurationOptions
				{
					AllowAdmin = true
				};
				config.EndPoints.Add("localhost:6379");
				Connection = ConnectionMultiplexer.Connect(config);
			}

			await Connection.GetServer("localhost:6379").FlushDatabaseAsync();
		}

		[TestMethod]
		public async Task GetSetCache()
		{
			await AssertGetSetCacheAsync(new RedisCacheLayer(Connection));
		}

		[TestMethod]
		public async Task IsCacheAvailable()
		{
			await AssertCacheAvailabilityAsync(new RedisCacheLayer(Connection), true);
		}

		[TestMethod]
		public async Task EvictFromCache()
		{
			await AssertCacheEvictionAsync(new RedisCacheLayer(Connection));
		}

		[TestMethod]
		public async Task CacheCleanup()
		{
			await AssertCacheCleanupAsync(new RedisCacheLayer(Connection));
		}

		[TestMethod]
		public async Task CachingComplexTypes()
		{
			await AssertComplexTypeCachingAsync(new RedisCacheLayer(Connection));
		}
	}
}
