using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace QueueServer
{
	public class HomeController : ApiController
	{
		[HttpPost]
		public async Task<Guid> Post(string text)
		{
			var task = new QueueTask
			{
				UniqueId = Guid.NewGuid(),
				Status = "Created",
				Content = text
			};

			DataStore.Instance.AddOrUpdate(task.UniqueId, task, (id, t) => task);
			MessageQueue.Instance.Enqueue(task);

			return task.UniqueId;
		}

		[HttpGet]
		public async Task<string> Check(Guid taskId)
		{
			QueueTask task;

			DataStore.Instance.TryGetValue(taskId, out task);
			if (null != task)
			{
				return task.Status;
			}

			return "NotExists";
		}

		[HttpGet]
		public async Task<HttpResponseMessage> GetResult(Guid taskId)
		{
			QueueTask task;

			var good = DataStore.Instance.TryGetValue(taskId, out task);

			if (good && null != task && task.Status == "Completed")
			{
				return Request.CreateResponse(HttpStatusCode.OK, task.Content);
			}

			return Request.CreateResponse(HttpStatusCode.NotFound);
		}

		[HttpPost]
		public async Task<QueueTask> GetTask()
		{
			QueueTask task;

			bool good = MessageQueue.Instance.TryDequeue(out task);

			if (good)
			{
				return task;
			}

			return null;
		}

		[HttpPost]
		public async Task<HttpResponseMessage> EditTask(QueueTask task)
		{
			if (null != task)
			{
				DataStore.Instance.Try
			}
		}
	}
}