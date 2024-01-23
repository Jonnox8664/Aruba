using Moq;
using Newtonsoft.Json.Linq;
using StateMachineWorkFlow;

namespace WFServiceTest
{
    public class WFTest
    {
        [Theory]
        [InlineData(-30, -50, 1)]
        [InlineData(10, 10, 1)]
        public void Invalid_ReceivedStateCode_ReceivedActionCode_KO(int receivedStateCode, int receivedActionCode, int currentStateCode)
        {
            // Arrange
            var jObject = new JObject()
            {
                { "ReceivedState", "ReceivedState" },
                { "ReceivedStateCode", receivedStateCode },
                { "ReceivedAction", "ReceivedAction" },
                { "ReceivedActionCode", receivedActionCode },
                { "CurrentState", "CurrentState" },
                { "CurrentStateCode", currentStateCode}
            };

            var moq = new Mock<IStateMachineManager<ValueTask>>();
            moq.Setup(x => x.StateMachineHandler(receivedStateCode, receivedActionCode)).Returns(jObject);

            // Act
            var result = moq.Object.StateMachineHandler(receivedStateCode, receivedActionCode);
            var actReceivedStateCode = (int?)result["ReceivedStateCode"];
            var actReceivedActionCode = (int?)result["ReceivedActionCode"];

            // Assert
            Assert.NotNull(result);
            Assert.True(actReceivedStateCode < 0 || actReceivedStateCode > 4);
            Assert.True(actReceivedActionCode < 0 || actReceivedActionCode > 4);
        }

        [Theory]
        [InlineData(0, 2, -10)]
        [InlineData(1, 1, 50)]
        public void Invalid_CurrentStateCode_KO(int receivedStateCode, int receivedActionCode, int currentStateCode)
        {
            // Arrange
            var jObject = new JObject()
            {
                { "ReceivedState", "ReceivedState" },
                { "ReceivedStateCode", receivedStateCode },
                { "ReceivedAction", "ReceivedAction" },
                { "ReceivedActionCode", receivedActionCode },
                { "CurrentState", "CurrentState" },
                { "CurrentStateCode", currentStateCode}
            };

            var moq = new Mock<IStateMachineManager<ValueTask>>();
            moq.Setup(x => x.StateMachineHandler(receivedStateCode, receivedActionCode)).Returns(jObject);

            // Act
            var result = moq.Object.StateMachineHandler(receivedStateCode, receivedActionCode);
            var actCurrentStateCode = (int?) result["CurrentStateCode"];

            // Assert
            Assert.NotNull(result);
            Assert.True(actCurrentStateCode < 0 || actCurrentStateCode > 4);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        [InlineData(2, 4)]
        public void ChangeState_Ok(int receivedState, int receivedStateCode)
        {
            // Arrange
            var smm = new StateMachineManager();

            // Act
            var result = smm.StateMachineHandler(receivedState, receivedStateCode);
            var newState = (int?)result["CurrentStateCode"];

            // Assert
            Assert.NotNull(newState);
            Assert.False(newState < 0);
            Assert.False(newState > 4);
        }

        // Start           ->      Create      ->      Created(Started)
        [Theory]
        [InlineData(0, 0)]
        public void ChangeState_Start_Create_Ok(int receivedState, int receivedStateCode)
        {
            // Arrange
            var smm = new StateMachineManager();

            // Act
            var result = smm.StateMachineHandler(receivedState, receivedStateCode);
            var newState = (int?)result["CurrentStateCode"];

            // Assert
            Assert.NotNull(newState);
            Assert.False(newState < 0);
            Assert.False(newState > 4);
            Assert.Equal(1, newState);
        }

        // Created         ->      Update      ->      Created
        [Theory]
        [InlineData(1, 1)]
        public void ChangeState_Created_Update_Ok(int receivedState, int receivedStateCode)
        {
            // Arrange
            var smm = new StateMachineManager();

            // Act
            var result = smm.StateMachineHandler(receivedState, receivedStateCode);
            var newState = (int?)result["CurrentStateCode"];

            // Assert
            Assert.NotNull(newState);
            Assert.False(newState < 0);
            Assert.False(newState > 4);
            Assert.Equal(1, newState);
        }

        // Created         ->      Handle      ->      InProgress
        [Theory]
        [InlineData(1, 2)]
        public void ChangeState_Created_Handle_Ok(int receivedState, int receivedStateCode)
        {
            // Arrange
            var smm = new StateMachineManager();

            // Act
            var result = smm.StateMachineHandler(receivedState, receivedStateCode);
            var newState = (int?)result["CurrentStateCode"];

            // Assert
            Assert.NotNull(newState);
            Assert.False(newState < 0);
            Assert.False(newState > 4);
            Assert.Equal(2, newState);
        }

        // InProgress      ->      Approve     ->      Approved     (Closed)
        [Theory]
        [InlineData(2, 3)]
        public void ChangeState_InProgress_Approve_Ok(int receivedState, int receivedStateCode)
        {
            // Arrange
            var smm = new StateMachineManager();

            // Act
            var result = smm.StateMachineHandler(receivedState, receivedStateCode);
            var newState = (int?)result["CurrentStateCode"];

            // Assert
            Assert.NotNull(newState);
            Assert.False(newState < 0);
            Assert.False(newState > 4);
            Assert.Equal(3, newState);
        }

        // InProgress      ->      Refuse      ->      Refused(Closed)
        [Theory]
        [InlineData(2, 4)]
        public void ChangeState_InProgress_Refuse_Ok(int receivedState, int receivedStateCode)
        {
            // Arrange
            var smm = new StateMachineManager();

            // Act
            var result = smm.StateMachineHandler(receivedState, receivedStateCode);
            var newState = (int?)result["CurrentStateCode"];

            // Assert
            Assert.NotNull(newState);
            Assert.False(newState < 0);
            Assert.False(newState > 4);
            Assert.Equal(4, newState);
        }

        // Start      ->      Refuse      ->      Refused(Closed)
        [Theory]
        [InlineData(0, 4)]
        public void ChangeState_Start_Refuse_kO(int receivedState, int receivedStateCode)
        {
            // Arrange
            var smm = new StateMachineManager();

            // Act
            var result = smm.StateMachineHandler(receivedState, receivedStateCode);
            var newState = (int?)result["CurrentStateCode"];

            // Assert
            Assert.NotNull(newState);
            Assert.Equal(-1, newState);
        }
    }
}