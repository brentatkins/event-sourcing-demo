using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Kanban.Projections
{
    public record KanbanBoardDto
    {
        public ImmutableArray<(StoryStatus Status, StoryDto Story)> Stories { get; init; }

        public ImmutableArray<StoryDto> Backlog =>
            this.Stories.Where(x => x.Status == StoryStatus.Backlog).Select(x => x.Story).ToImmutableArray();

        public ImmutableArray<StoryDto> InProgress =>
            this.Stories.Where(x => x.Status == StoryStatus.InProgress).Select(x => x.Story).ToImmutableArray();

        public ImmutableArray<StoryDto> UnderReview =>
            this.Stories.Where(x => x.Status == StoryStatus.UnderReview).Select(x => x.Story).ToImmutableArray();

        public ImmutableArray<StoryDto> ReadyForDeployment =>
            this.Stories.Where(x => x.Status == StoryStatus.ReadyForDeployment).Select(x => x.Story).ToImmutableArray();

        public ImmutableArray<StoryDto> Done
            => this.Stories.Where(x => x.Status == StoryStatus.Done).Select(x => x.Story).ToImmutableArray();

        public enum StoryStatus
        {
            Backlog,
            InProgress,
            UnderReview,
            ReadyForDeployment,
            Done
        }
    }

    public record StoryDto(string Id, string Title, DateTime DateCreated);
}