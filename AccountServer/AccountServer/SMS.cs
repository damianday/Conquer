using System;
using AlibabaCloud.OpenApiClient.Models;
using AlibabaCloud.SDK.Dysmsapi20170525;
using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using AlibabaCloud.TeaUtil.Models;

namespace AccountServer;

public class SMS
{
	public static Client client;

	public static Client CreateClient(string accessKeyId, string accessKeySecret)
	{
		Config config = new Config
		{
			AccessKeyId = accessKeyId,
			AccessKeySecret = accessKeySecret
		};
		config.Endpoint = "dysmsapi.aliyuncs.com";
		return new Client(config);
	}

	public static Client CreateClientWithSTS(string accessKeyId, string accessKeySecret, string securityToken)
	{
		Config config = new Config
		{
			AccessKeyId = accessKeyId,
			AccessKeySecret = accessKeySecret,
			SecurityToken = securityToken,
			Type = "sts"
		};
		config.Endpoint = "dysmsapi.aliyuncs.com";
		return new Client(config);
	}

	public static string Send(string phone, string code)
	{
		if (client == null)
		{
			client = CreateClient(Environment.GetEnvironmentVariable("ALIBABA_CLOUD_ACCESS_KEY_ID"), Environment.GetEnvironmentVariable("ALIBABA_CLOUD_ACCESS_KEY_SECRET"));
		}
		var request = new SendSmsRequest
		{
			SignName = "阿里云短信测试",
			TemplateCode = "SMS_154950909",
			PhoneNumbers = phone,
			TemplateParam = "{\"code\":\"" + code + "\"}"
		};
		SendSmsResponse sendSmsResponse = client.SendSms(request);
		return sendSmsResponse.Body.Message;
	}
}
