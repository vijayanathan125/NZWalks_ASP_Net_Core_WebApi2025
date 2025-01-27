using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalksTest.Helper
{
    public static class LoggerMockExtensions
    {
		public static void VerifyLog<T>(
		   this Mock<ILogger<T>> mockLogger,
		   LogLevel logLevel,
		   Func<Times> times,
		   string containsMessage = null)
		{
			mockLogger.Verify(
				x => x.Log(
					logLevel,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((o, t) =>
						containsMessage == null || o.ToString().Contains(containsMessage)),
					It.IsAny<Exception>(),
					(Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
				times);
		}
	}

}
