﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK;
using osu.Framework.Graphics;
using osu.Game.Storyboards.Drawables;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Storyboards
{
    public class SpriteDefinition : CommandTimelineGroup, IElementDefinition
    {
        public string Path { get; set; }
        public Anchor Origin;
        public Vector2 InitialPosition;

        private readonly List<CommandLoop> loops = new List<CommandLoop>();
        private readonly List<CommandTrigger> triggers = new List<CommandTrigger>();

        public SpriteDefinition(string path, Anchor origin, Vector2 initialPosition)
        {
            Path = path;
            Origin = origin;
            InitialPosition = initialPosition;
        }

        public CommandLoop AddLoop(double startTime, int loopCount)
        {
            var loop = new CommandLoop(startTime, loopCount);
            loops.Add(loop);
            return loop;
        }

        public CommandTrigger AddTrigger(string triggerName, double startTime, double endTime, int groupNumber)
        {
            var trigger = new CommandTrigger(triggerName, startTime, endTime, groupNumber);
            triggers.Add(trigger);
            return trigger;
        }

        public virtual Drawable CreateDrawable()
            => new StoryboardSprite(this);

        public override void ApplyTransforms(Drawable target, double offset = 0)
        {
            base.ApplyTransforms(target, offset);
            foreach (var loop in loops.OrderBy(l => l.StartTime))
                loop.ApplyTransforms(target, offset);
        }

        public override string ToString()
            => $"{Path}, {Origin}, {InitialPosition}";
    }
}
