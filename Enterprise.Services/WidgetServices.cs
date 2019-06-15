using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Enterprise.DAL;
using Enterprise.DTO;
using Enterprise.Models;

namespace Enterprise.Services
{
    public interface IWidgetServices
    {
        Task<List<WidgetGridDTO>> GetAll();
        Task UploadWidget(WidgetDTO newWidget);
        Task Delete(int id);
        Task<string> GetFeaturedImagePath(int id);
    }
    public class WidgetServices : IWidgetServices
    {
        private IRepository<Widgets> _repository;
        private IMapper _mapper;

        public WidgetServices(IRepository<Widgets> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<WidgetGridDTO>> GetAll()
        {
            var widgets = await _repository.Include(x => x.WidgetResources);
            var widgetsDTOList = new List<WidgetGridDTO>();
            _mapper.Map(widgets, widgetsDTOList);
            return widgetsDTOList;
        }
        public async Task UploadWidget(WidgetDTO newWidget)
        {
            var widget = new Widgets();
            _mapper.Map(newWidget, widget);
            await _repository.AddAsync(widget);
        }
        public async Task<string>GetFeaturedImagePath(int id)
        {
            var widget = await _repository.GetAsync(id, x => x.WidgetResources);
            var widgetFeatureImagePath = widget.WidgetResources.FeatureIamagePath;
            return widgetFeatureImagePath;
        }
        public async Task Delete(int id)
        {
            var widget = await _repository.GetAsync(id);
            await _repository.RemoveAsync(widget);
        }
    }
}
