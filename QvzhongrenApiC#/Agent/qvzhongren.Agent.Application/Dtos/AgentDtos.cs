using System.Text.Json.Serialization;

namespace qvzhongren.Agent.Application.Dtos;

public class AgentRequest
{
    public List<AgentMessage> Messages { get; set; } = new();
}

public class AgentMessage
{
    public string Role { get; set; } = "user";
    public string Content { get; set; } = "";
}

public class AgentResponse
{
    public string Content { get; set; } = "";
    public string? NavigateTo { get; set; }
    public List<AgentToolCall>? ToolCalls { get; set; }
}

public class AgentToolCall
{
    public string Name { get; set; } = "";
    public string Arguments { get; set; } = "";
    public string Result { get; set; } = "";
}

// DeepSeek API response types
public class DeepSeekResponse
{
    public List<DeepSeekChoice>? Choices { get; set; }
}

public class DeepSeekChoice
{
    public DeepSeekMessage? Message { get; set; }
}

public class DeepSeekMessage
{
    public string? Role { get; set; }
    public string? Content { get; set; }
    [JsonPropertyName("tool_calls")]
    public List<DeepSeekToolCall>? ToolCalls { get; set; }
}

public class DeepSeekToolCall
{
    public string Id { get; set; } = "";
    public string Type { get; set; } = "function";
    public DeepSeekFunction Function { get; set; } = new();
}

public class DeepSeekFunction
{
    public string Name { get; set; } = "";
    public string Arguments { get; set; } = "";
}
