using System.Collections.Generic;
using System.Linq;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.DTO.Mappers
{
    public static class KeywordDTOMapper
    {
        public static KeywordDTO ToDto(this Keyword keyword)
        {
            return keyword is null
                ? null
                : new KeywordDTO
                {
                    Id = keyword.Id,
                    Word = keyword.Word,
                };
        }
        public static Keyword FromDto(this KeywordDTO keyword)
        {
            return keyword is null
                ? null
                : new Keyword
                {
                    Id = keyword.Id,
                    Word = keyword.Word
                };
        }
        public static IEnumerable<KeywordDTO> ToDto(this IEnumerable<Keyword> keywords) => keywords.Select(ToDto);
        public static IEnumerable<Keyword> FromDto(this IEnumerable<KeywordDTO> keywords) => keywords.Select(FromDto);
    }
}
