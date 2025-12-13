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
            "Jesteś generatorem quizów.\n" +
            "Korzystaj WYŁĄCZNIE z podanego CONTEXT. Jeśli czegoś nie ma w kontekście — nie wymyślaj.\n" +
            "Zwróć WYŁĄCZNIE poprawny JSON. Bez markdown, bez komentarzy, bez dodatkowego tekstu.\n\n" +

            "Schemat JSON:\n" +
            "{\n" +
            "  \"title\": \"string\",\n" +
            "  \"tags\": [\"string\"],\n" +
            "  \"questions\": [\n" +
            "    {\n" +
            "      \"text\": \"string\",\n" +
            "      \"explanation\": \"string\",\n" +
            "      \"sourceChunkIndex\": 0,\n" +
            "      \"answers\": [\n" +
            "        { \"ordinal\": 0, \"text\": \"string\", \"isCorrect\": true }\n" +
            "      ]\n" +
            "    }\n" +
            "  ]\n" +
            "}\n\n" +

            "Zasady:\n" +
            $"- Wygeneruj {questionCount} pytań.\n" +
            "- Każde pytanie ma dokładnie 4 odpowiedzi.\n" +
            "- Dokładnie 1 odpowiedź ma isCorrect=true.\n" +
            "- ordinal ma być 0,1,2,3 w kolejności odpowiedzi.\n" +
            "- sourceChunkIndex musi odpowiadać indeksowi chunku widocznemu w CONTEXT ([Chunk X]).\n" +
            "- Wyjaśnienie krótkie: 1–2 zdania.\n";

        return new ChatMessage(ChatRole.System, text);
    }

    private static ChatMessage UserPrompt(string topic, string context)
    {
        // Wersja bez C# 11 (żeby było spójnie z SystemPrompt)
        var text =
            "Wygeneruj quiz wielokrotnego wyboru w języku polskim na temat:\n" +
            $"\"{topic}\"\n\n" +
            "CONTEXT (fragmenty wiedzy – użyj WYŁĄCZNIE tych informacji):\n" +
            $"{context}\n\n" +
            "Zwróć TYLKO poprawny JSON zgodny ze schematem podanym wcześniej.\n";

        return new ChatMessage(ChatRole.User, text);
    }
}