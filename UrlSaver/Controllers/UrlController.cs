using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlSaver.Data;
using UrlSaver.Services;
using UrlSaver.Models;
using UrlSaver.Dtos;

namespace UrlSaver.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly ILogger<UrlController> _logger;
        private readonly IUrlRepository _repository;
        private readonly IDistributedCacheService _cacheService;
        private readonly IUrlMetricsService _urlMetricsService;
        private readonly IMapper _mapper;

        public UrlController(
            ILogger<UrlController> logger, 
            IUrlRepository repository, 
            IDistributedCacheService cacheService,
            IUrlMetricsService urlMetricsService,
            IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _cacheService = cacheService;
            _urlMetricsService = urlMetricsService;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetAll))]
        public async Task<ActionResult<IEnumerable<UrlReadDto>>> GetAll ()
        {
            var cachedValue = await _cacheService.GetRecordAsync<IEnumerable<UrlReadDto>>("urls_all");

            if (cachedValue?.Any() == true)
            {
                _logger.LogInformation($"--> Result for action \"{nameof(GetAll)}\" from cache");

                return Ok(cachedValue);
            }

            var urls = _mapper.Map<IEnumerable<UrlReadDto>>(_repository.GetAll());
            await _cacheService.SetRecordAsync("urls_all", urls, TimeSpan.FromSeconds(30));

            _logger.LogInformation($"--> Result for action \"{nameof(GetAll)}\" from database");

            return Ok(urls);
        }

        [HttpGet("{id}", Name = nameof(GetById))]
        public async Task<ActionResult<UrlReadDto>> GetById ([FromRoute]Guid id)
        {
            var cachedValue = await _cacheService.GetRecordAsync<UrlReadDto>($"url_{id}");

            if (cachedValue != null)
            {
                _logger.LogInformation($"--> Result for action \"{nameof(GetById)}\" with Id {id} from cache");

                return Ok(cachedValue);
            }

            var url = _repository.GetByKey(id);

            if (url is null)
            {
                _logger.LogWarning($"--> Resource with Id {id} not found");

                return NotFound();
            }

            var urlReadDto = _mapper.Map<UrlReadDto>(url);
            await _cacheService.SetRecordAsync($"url_{id}", urlReadDto, TimeSpan.FromMinutes(5));

            _logger.LogInformation($"--> Result for action \"{nameof(GetById)}\" with Id {id} from database");

            return Ok(urlReadDto);
        }

        [HttpPost(Name = nameof(Create))]
        public async Task<ActionResult<UrlReadDto>> Create([FromBody] UrlCreateDto urlCreateDto)
        {
            var url = _mapper.Map<Url>(urlCreateDto);

            (url.TopDomain, url.SubDomains, url.Hostname) = _urlMetricsService.ProcessUrl(urlCreateDto.Original);

            await _repository.CreateAsync(url);
            await _repository.SaveAsync();

            var urlReadDto = _mapper.Map<UrlReadDto>(url);
            await _cacheService.SetRecordAsync($"url_{url.Id}", urlReadDto, TimeSpan.FromMinutes(5));

            _logger.LogInformation($"--> {nameof(Url)} created with Id {url.Id}");

            return CreatedAtRoute(nameof(GetById), new { url.Id }, urlReadDto);
        }
    }
}
