using ExtendedDatabase;
using NUnit.Framework;
using System;

namespace Tests
{
    public class ExtendedDatabaseTests
    {
        private ExtendedDatabase.ExtendedDatabase extendedDb;
        [SetUp]
        public void Setup()
        {
            extendedDb = new ExtendedDatabase.ExtendedDatabase();
        }

        [Test]
        public void Ctor_AddInitialPeopleToTheDb()
        {
            var persons = new Person[5];
            for (int i = 0; i < persons.Length; i++)
            {
                persons[i] = new Person(i, $"Name:{i}");
            }
            extendedDb = new ExtendedDatabase.ExtendedDatabase(persons);
            Assert.That(extendedDb.Count, Is.EqualTo(persons.Length));

            foreach (var person in persons)
            {
                Person dbPerson = extendedDb.FindById(person.Id);
                Assert.That(person, Is.EqualTo(dbPerson));
            }
        }

        [Test]
        public void Ctor_ThrowsExceptionWhenCapacityIsExceeded()
        {
            var persons = new Person[17];
            for (int i = 0; i < persons.Length; i++)
            {
                persons[i] = new Person(i, $"Peshe{i}");
            }
            Assert.Throws<ArgumentException>(()=> extendedDb=new ExtendedDatabase.ExtendedDatabase(persons));
        }

        [Test]
        public void Add_ThrowsException_WhenCoutIsExceeded()
        {
            var n = 16;
            for (int i = 0; i < n; i++)
            {
                extendedDb.Add(new Person(i, $"Name{i}"));
            }
            Assert.Throws<InvalidOperationException>(()=>extendedDb.Add(new Person(16, "Pesho")));
        }

        [Test]
        public void Add_ThrowsException_WhenUsernameAlreadyExists()
        {
            var name = "Pesho";
            extendedDb.Add(new Person(1,name));

            Assert.Throws<InvalidOperationException>(() => extendedDb.Add(new Person(16, name)));
        }

        [Test]
        public void Add_ThrowsException_WhenIdAlreadyExists()
        {
            var id = 1;
            extendedDb.Add(new Person(id, "Pesho"));

            Assert.Throws<InvalidOperationException>(() => extendedDb.Add(new Person(id, "Pesho")));
        }

        [Test]
        public void Add_IncrementCountWhenAllIsValid()
        {
            var expectedCount = 2;
            extendedDb.Add(new Person(1,"Pesho"));
            extendedDb.Add(new Person(2,"Gosho"));

            Assert.That(extendedDb.Count, Is.EqualTo(expectedCount));
        }

        [Test]
        public void Remove_ThrowsExceptionWheDbIsEmpty()
        {
            Assert.Throws<InvalidOperationException>(() => extendedDb.Remove());
        }

        [Test]
        public void Remove_ElementFromDb()
        {
            var n = 3;
            for (int i = 0; i < n; i++)
            {
                extendedDb.Add(new Person(i, $"Pesho{i}"));
            }
            extendedDb.Remove();
            Assert.That(extendedDb.Count, Is.EqualTo(n-1));
            Assert.Throws<InvalidOperationException>(()=>extendedDb.FindById(n-1));
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void FindByUsername_ThrowsExceptionWhenUsernameIsInvalid(string username)
        {
            Assert.Throws<ArgumentNullException>(()=>extendedDb.FindByUsername(username));
        }

        [Test]
        public void FindByUsername_ThrowsExceptionWhenUsernameIsNotExisting()
        {
            Assert.Throws<InvalidOperationException>(() => extendedDb.FindByUsername("dadane"));
        }

        [Test]
        public void FindByUsername_ReturnsTheCorrectResult()
        {
            var person = new Person(1, "Pesho");
            extendedDb.Add(person);
            var dbPerson = extendedDb.FindByUsername(person.UserName);

            Assert.That(person, Is.EqualTo(dbPerson));
        }

        [Test]
        public void FindById_ThrowExceptionForInvalidId()
        {
            Assert.Throws<InvalidOperationException>(()=>extendedDb.FindById(123));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-21)]
        public void FindById_ThrowExceptionWhenIdIsNegative(int id)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => extendedDb.FindById(id));
        }

        [Test]
        public void FindById_ReturnsCorrectResult()
        {
            var person = new Person(1,"Pesho");
            extendedDb.Add(person);
            var dbPerson = extendedDb.FindById(person.Id);
            Assert.That(person, Is.EqualTo(dbPerson));
        }
    }
}