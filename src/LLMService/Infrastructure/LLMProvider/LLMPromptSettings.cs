using Microsoft.Extensions.AI;

namespace LLMService.Infrastructure.LLMProvider;

public static class LLMPromptSettings
{
    public static IReadOnlyList<ChatMessage> BuildQuizMessages(
        int questionCount,
        string topic,
        string context)
        => new[]
        {
            SystemPrompt(questionCount),
            UserPrompt(topic, context)
        };

    private static ChatMessage SystemPrompt(int questionCount)
    {
        var text =
            "You are a quiz generator.\n" +
            "Use ONLY the provided CONTEXT. If something is not in the context — do not make it up.\n\n" +

            "You MUST generate a complete JSON with ALL fields filled:\n" +
            "- title: string (required, in Polish)\n" +
            "- tags: array of strings (required, in Polish)\n" +
            "- questions: array (required, EXACTLY " + questionCount + " items)\n\n" +

            "Each question MUST have ALL these fields:\n" +
            "- text: string (required, the question in Polish)\n" +
            "- explanation: string (required, 1–2 sentences in Polish; this must be a subtle hint or contextual clue that helps reasoning, but it MUST NOT directly reveal or explicitly point to the correct answer)\n" +
            "- sourceChunkIndex: integer (required, index from CONTEXT [Chunk X])\n" +
            "- answers: array of exactly 4 objects (required)\n\n" +

            "Each answer MUST have ALL these fields:\n" +
            "- ordinal: integer (required, 0, 1, 2, or 3)\n" +
            "- text: string (required, answer text in Polish)\n" +
            "- isCorrect: boolean (required, exactly one true per question)\n\n" +

            "CRITICAL RULES:\n" +
            "- Generate EXACTLY " + questionCount + " complete questions.\n" +
            "- NEVER skip any field - all fields are mandatory.\n" +
            "- All text content MUST be in Polish.\n" +
            "- The explanation MUST function as a hint, not an answer: it cannot name, restate, or obviously identify the correct option.\n";

        return new ChatMessage(ChatRole.System, text);
    }

    private static ChatMessage UserPrompt(string topic, string context)
    {
        var text =
            $"Generate a multiple choice quiz with MULTIPLE questions about:\n" +
            $"\"{topic}\"\n\n" +
            "CONTEXT (knowledge fragments — use ONLY this information):\n" +
            $"{context}\n\n" +
            "Remember:\n" +
            "- All output text must be in Polish.\n" +
            "- Generate ALL required questions, not just one.\n";

        return new ChatMessage(ChatRole. User, text);
    }
}