using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class AttackFactory
    {
        public static PetAttackBase GetAttack(AttackNameId nome)
        {
            PetAttackBase retorno;
            switch (nome)
            {
                case AttackNameId.rajadaDeAgua:
                    retorno = new RajadaDeAgua();
                break;

                case AttackNameId.turboDeAgua:
                    retorno = new TurboDeAgua();
                break;
                case AttackNameId.tapa:
                    retorno = new Tapa();
                break;

                case AttackNameId.laminaDeFolha:
                    retorno = new LaminaDeFolhas();
                break;

                case AttackNameId.garra:
                    retorno = new Garra();
                break;

                case AttackNameId.furacaoDeFolhas:
                    retorno = new FuracaoDeFolhas();
                break;

                case AttackNameId.bolaDeFogo:
                    retorno = new BolaDeFogo();
                break;

                case AttackNameId.rajadaDeFogo:
                    retorno = new RajadaDeFogo();
                break;
                case AttackNameId.ventania:
                    retorno = new Ventania();
                break;
                case AttackNameId.bico:
                    retorno = new Bico();
                break;
                case AttackNameId.ventosCortantes:
                    retorno = new VentosCortantes();
                break;
                //case nomesGolpes.chicoteDeCalda:
                //    retorno = new ChicoteDeCaldaG2();
                //    break;
                //case nomesGolpes.gosmaDeInseto:
                //    retorno = new GosmaDeInsetoG2();
                //    break;
                //case nomesGolpes.gosmaAcida:
                //    retorno = new GosmaAcidaG2();
                //    break;
                //case nomesGolpes.psicose:
                //    retorno = new PsicoseG2();
                //    break;
                //case nomesGolpes.bolaPsiquica:
                //    retorno = new BolaPsiquicaG2();
                //    break;
                //case nomesGolpes.chicoteDeMao:
                //    retorno = new MbChicoteDeMao();
                //    break;
                case AttackNameId.dentada:
                    retorno = new Dentada();
                break;
                //case nomesGolpes.sobreSalto:
                //    retorno = new MbSobreSalto();
                //    break;
                //case nomesGolpes.agulhaVenenosa:
                //    retorno = new MbAgulhaVenenosa();
                //    break;
                //case nomesGolpes.ondaVenenosa:
                //    retorno = new MbOndaVenenosa();
                //    break;
                //case nomesGolpes.bastao:
                //    retorno = new MbBastao();
                //    break;
                //case nomesGolpes.pedregulho:
                //    retorno = new MbPedregulho();
                //    break;
                //case nomesGolpes.cascalho:
                //    retorno = new MbCascalho();
                //    break;
                //case nomesGolpes.cabecada:
                //    retorno = new MbCabecada();
                //    break;
                //case nomesGolpes.chute:
                //    retorno = new MbChute();
                //    break;
                //case nomesGolpes.espada:
                //    retorno = new MbEspada();
                //    break;
                //case nomesGolpes.chifre:
                //    retorno = new MbChifre();
                //    break;

                case AttackNameId.tempestadeDeFolhas:
                    retorno = new TempestadeDeFolhas();
                break;

                //case nomesGolpes.eletricidade:
                //    retorno = new MbEletricidade();
                //    break;
                //case nomesGolpes.eletricidadeConcentrada:
                //    retorno = new MbEletricidadeConcentrada();
                //    break;
                //case nomesGolpes.tempestadeEletrica:
                //    retorno = new MbTempestadeEletrica();
                //    break;

                case AttackNameId.hidroBomba:
                    retorno = new HidroBomba();
                break;

                case AttackNameId.tosteAtaque:
                    retorno = new TosteAtaque();
                break;

                //case nomesGolpes.chuvaVenenosa:
                //    retorno = new MbChuvaVenenosa();
                //    break;
                //case nomesGolpes.rajadaDeTerra:
                //    retorno = new MbRajadaDeTerra();
                //    break;
                //case nomesGolpes.vingancaDaTerra:
                //    retorno = new MbVingancaDaTerra();
                //    break;
                //case nomesGolpes.cortinaDeTerra:
                //    retorno = new MbCortinaDeTerra();
                //    break;
                //case nomesGolpes.rajadaDeGas:
                //    retorno = new MbRajadaDeGas();
                //    break;
                //case nomesGolpes.bombaDeGas:
                //    retorno = new MbBombaDeGas();
                //    break;
                //case nomesGolpes.cortinaDeFumaca:
                //    retorno = new MbCortinaDeFumaca();
                //    break;
                //case nomesGolpes.anelDoOlhar:
                //    retorno = new MbAnelDoOlhar();
                //    break;
                //case nomesGolpes.teletransporte:
                //    retorno = new MbTeletransporte();
                //    break;
                //case nomesGolpes.avalanche:
                //    retorno = new MbAvalanche();
                //    break;
                //case nomesGolpes.multiplicar:
                //    retorno = new MbMultiplicar();
                //    break;
                case AttackNameId.sobreVoo:
                    retorno = new SobreVoo();
                break;
                //case nomesGolpes.sabreDeAsa:
                //case nomesGolpes.sabreDeBastao:
                //case nomesGolpes.sabreDeEspada:
                //case nomesGolpes.sabreDeNadadeira:
                //    retorno = new Sabre(nome);
                //    break;
                //case nomesGolpes.olharMal:
                //    retorno = new OlharMal();
                //    break;
                //case nomesGolpes.olharEnfraquecedor:
                //    retorno = new OlharEnfraquecedor();
                //    break;
                default:
                    retorno = new NullPetAttack(new PetAttackFeatures());
                break;
            }
            return retorno;
        }
    }
}