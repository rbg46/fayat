using FluentAssertions;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Fred.Business.Referential.CodeDeplacement;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace Fred.Business.Tests
{
    [TestClass]
    public class TestCodeDeplacementValidator
    {
        private Mock<ICodeDeplacementRepository> codeDeplacementRepositoryFake;
        private Mock<IUnitOfWork> uow;
        private CodeDeplacementValidator validator;
        private string ErrorCodeNotEmptyMessage = "NotEmptyValidator";
        private string ErrorCodeLengthMessage = "LengthValidator";

        [TestInitialize]
        public void ArrangeDependenciesAndCreateValidator()
        {
            this.uow = new Mock<IUnitOfWork>();
            this.codeDeplacementRepositoryFake = new Mock<ICodeDeplacementRepository>();
            this.validator = new CodeDeplacementValidator(this.uow.Object, this.codeDeplacementRepositoryFake.Object);
        }


        /// <summary>
        ///   Validation de la longueur maximale du champ Code (cas passant)
        /// </summary>
        [TestMethod]
        public void Verify_Code_Ok()
        {
            //ASSERT
            CodeDeplacementEnt codeDeplacementEnt = new CodeDeplacementEnt { Code = "0123456789" };
            this.validator.ShouldNotHaveValidationErrorFor(codeDeplacement => codeDeplacement.Code, codeDeplacementEnt);
        }

        /// <summary>
        ///   Validation de la longueur maximale du champ Code (cas non passant)
        /// </summary>
        [TestMethod]
        public void Verify_Code_Max_Lenght_Error()
        {
            //ASSERT
            CodeDeplacementEnt codeDeplacementEnt = new CodeDeplacementEnt { Code = "0123456789 0123456789 0123456789" };
            this.validator.ShouldHaveValidationErrorFor(codeDeplacement => codeDeplacement.Code, codeDeplacementEnt);
        }


        /// <summary>
        ///   Validation de la presence de valeur du champ Code (cas non passant)
        /// </summary>
        [TestMethod]
        public void Verify_Code_Empty_Error()
        {
            //ARRANGE
            CodeDeplacementEnt codeDeplacementEnt = new CodeDeplacementEnt { Code = null };

            //ACT
            ValidationResult validationResult = this.validator.Validate(new CodeDeplacementEnt { Code = null });

            //ASSERT
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().Contain(v => v.ErrorCode == ErrorCodeNotEmptyMessage && v.PropertyName == "Code");
        }

        /// <summary>
        ///   Validation du champ Libelle (cas passant)
        /// </summary>
        [TestMethod]
        public void Verify_Libelle_Ok()
        {
            //ARRANGE
            CodeDeplacementEnt codeDeplacementEnt = new CodeDeplacementEnt { Libelle = "libelleOk" };

            //ACT
            ValidationResult validationResult = this.validator.Validate(codeDeplacementEnt);

            //ASSERT
            validationResult.Errors.Should().NotContain(v => v.PropertyName == "Libelle");
        }

        /// <summary>
        ///   Validation de la longueur maximale du champ Libelle (cas non passant)
        /// </summary>
        [TestMethod]
        public void Verify_Libelle_Max_Lenght_Error()
        {
            string moreMaxLenghtValue = new string('0', 700);

            //ARRANGE
            CodeDeplacementEnt codeDeplacementEnt = new CodeDeplacementEnt { Libelle = moreMaxLenghtValue };

            //ACT
            ValidationResult validationResult = this.validator.Validate(codeDeplacementEnt);

            //ASSERT
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().Contain(v => v.PropertyName == "Libelle" && v.ErrorCode == ErrorCodeLengthMessage);
        }

        /// <summary>
        ///   Validation de la presence de valeur du champ Libelle (cas non passant)
        /// </summary>
        [TestMethod]
        public void Verify_Libelle_Empty_Error()
        {
            //ARRANGE
            CodeDeplacementEnt codeDeplacementEnt = new CodeDeplacementEnt { Libelle = null };

            //ACT
            ValidationResult validationResult = this.validator.Validate(codeDeplacementEnt);

            //ASSERT
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().Contain(v => v.PropertyName == "Libelle" && v.ErrorCode == ErrorCodeNotEmptyMessage);
        }

        /// <summary>
        ///   Validation que le KmMini est inferieur au KmMax (cas passant)
        /// </summary>
        [TestMethod]
        public void Verify_KmMini_Is_Inferior_Or_Equal_To_KmMaxi_OK()
        {
            //ARRANGE
            CodeDeplacementEnt codeDeplacementEnt = new CodeDeplacementEnt { KmMini = 10, KmMaxi = 100 };

            //ACT
            ValidationResult validationResult = this.validator.Validate(codeDeplacementEnt);

            //ASSERT
            validationResult.Errors.Should().NotContain(v => v.PropertyName == "KmMini");
        }

        /// <summary>
        ///   Validation que le KmMini est inferieur au KmMax (cas non passant)
        /// </summary>
        [TestMethod]
        public void Verify_KmMini_Is_Inferior_Or_Equal_To_KmMaxi_Error()
        {
            //ARRANGE
            CodeDeplacementEnt codeDeplacementEnt = new CodeDeplacementEnt { KmMini = 1000, KmMaxi = 100 };

            //ACT
            ValidationResult validationResult = this.validator.Validate(codeDeplacementEnt);

            //ASSERT
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().Contain(v => v.PropertyName == "KmMini");
        }

        /// <summary>
        ///   Validation que le code n'existe pas en base (cas passant)
        /// </summary>
        [TestMethod]
        public void Verify_CodeExistInSocieteError_When_Code_Not_Exist_For_the_same_compagnie_Ok()
        {
            this.codeDeplacementRepositoryFake
                .Setup(cdr => cdr.CodeDeplacementExistForSocieteIdAndCode(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);

            //ARRANGE
            CodeDeplacementEnt codeDeplacementEnt = new CodeDeplacementEnt();

            //ACT
            ValidationResult validationResult = this.validator.Validate(codeDeplacementEnt);

            //ASSERT
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().Contain(v => v.PropertyName == CodeDeplacementValidator.CodeExistInSocieteErrorKey);
        }

        /// <summary>
        ///   Validation que le code n'existe pas en base (cas non passant)
        /// </summary>
        [TestMethod]
        public void Verify_CodeExistInSocieteError_When_Already_Code_Exist_For_the_same_compagnie_Error()
        {
            this.codeDeplacementRepositoryFake
                .Setup(cdr => cdr.CodeDeplacementExistForSocieteIdAndCode(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(false);

            //ARRANGE
            CodeDeplacementEnt codeDeplacementEnt = new CodeDeplacementEnt();

            //ASSERT
            this.validator.ShouldNotHaveValidationErrorFor(codeDeplacement => CodeDeplacementValidator.CodeExistInSocieteErrorKey, codeDeplacementEnt);
        }
    }
}
