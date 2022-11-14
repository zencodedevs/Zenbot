using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.Domain.Shared.Entities.Bot.Dtos
{
    public class BoardingFilesDto
    {
        public int Id { get; set; }
        public List<IFormFile> FilePaths { get; set; }

        public int BoardingMessageId { get; set; }
    }
}
