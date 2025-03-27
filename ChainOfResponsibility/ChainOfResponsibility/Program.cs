namespace ChainOfResponsibility
{
    #region Method Chain
    public class Creature
    {
        public string Name;
        public int Attack, Defence;

        public Creature(string name, int attack, int defence)
        {
            Name = name;
            Attack = attack;
            Defence = defence;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defence)}: {Defence}";
        }
    }

    public class CreatureModifier
    {
        protected Creature creature;
        protected CreatureModifier next; // linked list => Class inside class inside class .....

        public CreatureModifier(Creature creature)
        {
            this.creature = creature;
        }

        public void Add(CreatureModifier modifier)
        {
            if (next != null) next.Add(modifier);
            else next = modifier;
        }

        public virtual void Handle() => next?.Handle();
    }

    public class NoBonusesModifier : CreatureModifier
    {
        public NoBonusesModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            Console.WriteLine("No bonuses for you!");
        }
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            Console.WriteLine($"Doubling {creature.Name}'s attack");
            creature.Attack *= 2;
            base.Handle();
        }
    }

    public class IncreaseDefenceModifier : CreatureModifier
    {
        public IncreaseDefenceModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            Console.WriteLine($"Increasing {creature.Name}'s Defence");
            creature.Defence += 3;
            base.Handle();
        }
    }
    #endregion


    #region Broker Chain
    // We will use mediator pattern
    public class Game
    {
        public event EventHandler<Query> Queries;

        public void PerformQuery(object sender, Query query)
        {
            Queries?.Invoke(sender, query);
        }
    }

    public class Query
    {
        public string CreatureName;
        public enum Argument
        {
            Attack, Defence
        }

        public Argument WhatToQuery;

        public int Value;

        public Query(string creatureName, Argument wahtToQuery, int value)
        {
            CreatureName = creatureName;
            WhatToQuery = wahtToQuery;
            Value = value;
        }
    }

    public class Creature1
    {
        private Game game;
        public string Name;
        private int attack, defence;

        public Creature1(Game game, string name, int attack, int defence)
        {
            this.game = game;
            Name = name;
            this.attack = attack;
            this.defence = defence;
        }

        public int Attack
        {
            get
            {
                var query = new Query(Name, Query.Argument.Attack, attack);
                game.PerformQuery(this, query); // query.value
                return query.Value;
            }
        }

        public int Defence
        {
            get
            {
                var query = new Query(Name, Query.Argument.Defence, defence);
                game.PerformQuery(this, query); // query.value
                return query.Value;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defence)}: {Defence}";
        }
    }

    public abstract class CreatureModifier1 : IDisposable
    {
        protected Game game;
        protected Creature1 creature1;

        protected CreatureModifier1(Game game, Creature1 creature1)
        {
            this.game = game;
            this.creature1 = creature1;
            game.Queries += Handle;
        }

        protected abstract void Handle(object sender, Query query);

        public void Dispose()
        {
            game.Queries -= Handle;
        }
    }

    public class DoubleAttackModifier1 : CreatureModifier1
    {
        public DoubleAttackModifier1(Game game, Creature1 creature1) : base(game, creature1)
        {
        }

        protected override void Handle(object sender, Query query)
        {
            if (query.CreatureName == creature1.Name && query.WhatToQuery == Query.Argument.Attack)
                query.Value *= 2;
        }
    }

    public class IncreaseDefenceModifier1 : CreatureModifier1
    {
        public IncreaseDefenceModifier1(Game game, Creature1 creature1) : base(game, creature1)
        {
        }

        protected override void Handle(object sender, Query query)
        {
            if (query.CreatureName == creature1.Name && query.WhatToQuery == Query.Argument.Defence)
                query.Value += 3;
        }
    } 
    #endregion

    public class Program
    {
        static void Main(string[] args)
        {
            #region Method Chain
            var goblin = new Creature("Goblin", 2, 2);
            Console.WriteLine(goblin);

            var root = new CreatureModifier(goblin);

            root.Add(new NoBonusesModifier(goblin));

            Console.WriteLine("Let's double the goblin's attack!");
            root.Add(new DoubleAttackModifier(goblin));

            root.Add(new DoubleAttackModifier(goblin));
            root.Add(new DoubleAttackModifier(goblin));

            Console.WriteLine("Let's Increase goblin defence!");
            root.Add(new IncreaseDefenceModifier(goblin));
            root.Add(new IncreaseDefenceModifier(goblin));

            root.Handle();

            Console.WriteLine(goblin);
            #endregion

            #region Broker Chain
            var game = new Game();
            var goblin1 = new Creature1(game, "Strong Goblin1", 2, 2);
            Console.WriteLine(goblin1);

            using (new DoubleAttackModifier1(game, goblin1))
            {
                new DoubleAttackModifier1(game, goblin1);
                new DoubleAttackModifier1(game, goblin1);
                Console.WriteLine(goblin1);
            }

            using (new IncreaseDefenceModifier1(game, goblin1))
            {
                new IncreaseDefenceModifier1(game, goblin1);
                new IncreaseDefenceModifier1(game, goblin1);
                Console.WriteLine(goblin1);
            }

            Console.WriteLine(goblin1); 
            #endregion
        }
    }
}
