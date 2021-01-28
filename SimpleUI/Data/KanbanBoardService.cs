using System.Threading.Tasks;
using EventSourcing;
using Kanban;
using Kanban.Projections;
using shortid;
using shortid.Configuration;

namespace SimpleUI.Data
{
    public class KanbanBoardService
    {
        private readonly IEntityRepository _entityRepository;
        private readonly KanbanBoardLiveProjection _projection;

        public KanbanBoardService(IEntityRepository entityRepository, KanbanBoardLiveProjection projection)
        {
            _entityRepository = entityRepository;
            _projection = projection;
        }

        public async Task CreateStory(string title)
        {
            var newId = ShortId.Generate(new GenerationOptions
                {UseNumbers = false, UseSpecialCharacters = false, Length = 8});

            var newStory = Story.Create(title, newId);

            await _entityRepository.Save(newStory);
        }

        public KanbanBoardDto GetBoardState()
        {
            return _projection.State;
        }

        public async Task MoveToInProgress(string storyId)
        {
            var story = await _entityRepository.Get<Story>(storyId);
            story.MoveToInProgress();

            await _entityRepository.Save(story);
        }

        public async Task MoveToUnderReview(string storyId)
        {
            var story = await _entityRepository.Get<Story>(storyId);
            story.MoveToInReview();

            await _entityRepository.Save(story);
        }
        
        public async Task MoveToReadyForDeployment(string storyId)
        {
            var story = await _entityRepository.Get<Story>(storyId);
            story.MoveToReadyForDeployment();

            await _entityRepository.Save(story);
        }
        
        public async Task MoveToDone(string storyId)
        {
            var story = await _entityRepository.Get<Story>(storyId);
            story.MoveToDone();

            await _entityRepository.Save(story);
        }

        public async Task DeleteStory(string storyId)
        {
            var story = await _entityRepository.Get<Story>(storyId);
            story.Delete();

            await _entityRepository.Save(story);
        }
    }
}